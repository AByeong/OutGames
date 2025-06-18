using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;


public class FirebaseTest : MonoBehaviour
{
    private FirebaseFirestore _db ;
    private FirebaseApp _app;
    private FirebaseAuth _auth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
Init();        
    }

    
    //파이어베이스 내 프로젝트에 연결
    void Init()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                Debug.Log("파이어베이스 연결 성공!");
                
                _app = Firebase.FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                _db = FirebaseFirestore.DefaultInstance;
                
                Login();
            } else {
                Debug.LogError($"파이어베이스 연결 실패! ${dependencyStatus}");
            }
        });
    }

    private void Register()
    {
        string email = "Test@gamil.com";
        string password = "123456";
        
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.LogError("회원가입에 실패하였습니다");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("오 마이 갓!" + task.Exception + "이 발생함!!");
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("회원가입에 성공했습니다: {0} ({1})", result.User.DisplayName, result.User.UserId);
            return ;
        });
    }

    private void Login()
    {
        string email = "Test@gamil.com";
        string password = "123456";
        
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.LogError($"로그인에 실패했어용 : {task.Exception.Message}");
                return;
            }
         
            

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("로그인 성공!: {0} ({1})", result.User.DisplayName, result.User.UserId);
            
            NicknameChange();
            AddMyRanking();
        });
    }

    private void NicknameChange()
    {
        Firebase.Auth.FirebaseUser user = _auth.CurrentUser;
        if (user == null)
        {
            return;
        }

        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
                DisplayName = "Teemo",
            };
        
        
            user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
                if (task.IsCanceled) {
                    Debug.LogError("닉넴변경실패.");
                    return;
                }
              

                Debug.Log("닉넴 변경 성공.");
            });
        
    }

    private void GetProfile()
    {
        Firebase.Auth.FirebaseUser user = _auth.CurrentUser;
        if (user == null)
        {
            return;
        }

        string nickname = user.DisplayName;
        string email = user.Email;
            
        Account account = new Account(email, nickname, "firebase");        
        
    }

    private void AddMyRanking()
    {
        Rank ranking = new Rank(1972,"testing@gmail.com","곶곶곶");
        Dictionary<string, object> rankingDict = new Dictionary<string, object>
        {
            { "Email", ranking.Email },
            { "Nickname", ranking.Nickname },
            {"Score", ranking.Score },
        };
        _db.Collection("rankings").Document(ranking.Email).SetAsync(rankingDict).ContinueWithOnMainThread(task => {
            
            Debug.Log(String.Format("Added Or Updated document with ID: {0}.", task.Id));
        });
        
        GetMyRankings();
    }

    private void GetMyRankings()
    {
        string email = "testing@gmail.com"; //ID 역할
        
        DocumentReference docRef = _db.Collection("rankings").Document(email);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            
            if (snapshot.Exists) {
                
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                var rankingDict = snapshot.ToDictionary();
                
                foreach (KeyValuePair<string, object> pair in rankingDict) 
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }
            } 
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }
}
