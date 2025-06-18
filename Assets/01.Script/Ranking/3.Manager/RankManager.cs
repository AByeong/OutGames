using System;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

public class RankManager : MonoBehaviour
{
   public GameObject RankUI;
   public static RankManager Instance;

   public List<RankDTO> Ranking;
   public Rank PlayerRank;
   private RankRepository _rankRepository;
   
   public event Action OnDataChanged;
   private AccountDTO _account;
   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      else
      {
         Destroy(this);
      }


      Init();
   }

   private void Init()
   {
      _rankRepository = new RankRepository();
      Ranking = _rankRepository.LoadServerData();
      
      //Repo에서 PlayerDTO를 Load 받아옴
      if (!TryGetPlayerDto())
      {
         PlayerRank = new Rank(0, "test@test.com", _account.Nickname); //RankNumber를 처음에 0으로 하는 것에 대해서 논의가 필요함
      }
      
      EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
   }

   private void OnPlayerDeath(PlayerDeathEvent evt) => ShowUI();

   private void ShowUI()
   {
      _rankRepository.Save(PlayerRank.ToDTO());
      RankUI.SetActive(true);
      OnDataChanged?.Invoke();

   }

   public List<RankDTO> Load()
   {
      return _rankRepository.LoadServerData();
   }
   public bool TryGetPlayerDto()
   {

      _account = AccountManager.Instance.CurrentAccount;
      if (_rankRepository == null)
      {
         throw new Exception("랭크리포지토리가 null입니당");
      }
      RankDTO playerRankDto = _rankRepository.Load(_account.Nickname);

      if (playerRankDto == null)
      {
         return false;
      }
      
      PlayerRank = new Rank(playerRankDto);

      return true;
   }
   
}
