using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class RankManager : MonoBehaviourSingleton<RankManager>
{
    private RankingRepository _rankingRepository;
    private List<Ranking> _rankings;
    private Ranking _myRanking;
    
    public event Action OnDataChanged;
    
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        _rankingRepository = new RankingRepository();
        
        List<RankingSaveData> saveDataList = _rankingRepository.Load();
        foreach (RankingSaveData saveData in saveDataList)
        {
            Ranking ranking = new Ranking(saveData.Score, saveData.Nickname, saveData.Email);
            _rankings.Add(ranking);

            if (ranking.Email == AccountManager.Instance.CurrentAccount.Email)
            {
                _myRanking = ranking;
            }
            
        }

        if (_myRanking == null)
        {
            AccountDTO me = AccountManager.Instance.CurrentAccount;
            _myRanking = new Ranking(0, me.Nickname, me.Email);
            
            _rankings.Add(_myRanking);
        }
        
        Sort();
        
        OnDataChanged?.Invoke();
        
        
    }


    private void Sort()
    {
        _rankings.Sort((r1,r2) =>r1.Score.CompareTo(r2.Score));

        for (int i = 0; i < _rankings.Count; i++)
        {
            _rankings[i].SetRank(i+1);
        }
    }
}
