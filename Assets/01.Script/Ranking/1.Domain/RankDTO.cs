using System;
using UnityEngine;

[Serializable]
public class RankDTO 
{
   public readonly int Score;
   public readonly string Email;
   public readonly string Nickname;
   
   
   public RankDTO(int score, string email, string nickname)
   {
      if (score < 0)
      {
         throw new Exception("점수는 음수가 될 수 없습니다.");
      }

      if (string.IsNullOrEmpty(email))
      {
         throw new Exception("점수는 음수가 될 수 없습니다.");
      }

      if (string.IsNullOrEmpty(nickname))
      {
         throw new Exception("닉넴이 비었어용");
      }
      
      this.Score = score;
      this.Email = email;
      this.Nickname = nickname;
   }
}
