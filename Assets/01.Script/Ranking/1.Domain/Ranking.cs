using System;
using UnityEngine;

public class Ranking 
{
   public int Rank { get; private set; }
   public readonly string Email;
   public readonly string Nickname;
   public int Score { get; private set; }

   public Ranking(int score, string nickname, string email)
   {
     
      
      AccountEmailSpecification accountEmailSpecification = new AccountEmailSpecification();
      if (!accountEmailSpecification.IsSatisfiedBy(email))
      {
         throw new Exception(accountEmailSpecification.ErrorMassage);
      }
      
      
      AccountNicknameSpecification accountNicknameSpecification = new AccountNicknameSpecification();
      if (!accountNicknameSpecification.IsSatisfiedBy(nickname))
      { 
         throw new Exception(accountNicknameSpecification.ErrorMassage);
      }
      
      

      if (score < 0)
      {
         throw new Exception("올바르지 못한 점수입니다.");
      }
      
      Email = email;
      Nickname = nickname;
      Score = score;
   }


   public void SetRank(int rank)
   {
      if (rank <= 0)
      {
         throw new Exception("올바르지 못한 등수");
      }
      
      Rank = rank;
   }

   public void AddScore(int score)
   {
      if (score < 0)
      {
         throw new Exception("올바르지 못한 점수입니다.");
      }
      
      Score += score;
   }
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
}
