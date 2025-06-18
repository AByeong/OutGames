using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
   
   public static StageManager instance;
   

   private float _timer;
   public float Timer => _timer;
   private Stage _stage;
   public event Action OnStageLevelChanged;
   public List<StageLevelSO> StageLevelSOs = new List<StageLevelSO>();
private StageRepo _stageRepo = new StageRepo();
   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
      else
      {
         Destroy(this);
      }
   }
   
   private void Start()
   {
      Init();
   }

   void Init()
   {
      if (_stageRepo.Load() == null)
      {
         _stage = new Stage(0,0);
         _timer = 0;
      }
      else
      {
         _stage = new Stage(_stageRepo.Load());
         _timer = 0;

      }
      
      
      foreach (StageLevelSO stageSO in StageLevelSOs)
      {
         _stage.AddStageLevel(stageSO);
      }
      _stage.TryCheckStageLevel(_timer);

       
      Debug.Log("커런트 스테이지의 타입 : "+_stage.CurrentStageLevel.StageLevelType.ToString());
   }
   
   private void Update()
   {
      _timer += Time.deltaTime;
      Check();
   }

   void Check()
   {
      if (!_stage.TryCheckStageLevel(_timer)) return;
      _stageRepo.Save();
      OnStageLevelChanged?.Invoke();
      Debug.Log("바뀜");

   }

   public StageDTO GetCurrentStageDTO()
   {
      return new StageDTO(_stage);
   }
   
}
