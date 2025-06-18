using System.Collections.Generic;
using System;
using UnityEngine;


public class RankRepository
{
    public List<RankDTO> RankList { get; private set; }
    public RankDTO PlayerRank { get; private set; }

    private const string SAVE_SERVER_KEY = nameof(RankRepository) + "_SERVER";

    public RankRepository()
    {
        RankList = new List<RankDTO>();
        RankList = LoadServerData();
    }

    public void Save(RankDTO playerRank)
    {
        if (playerRank == null)
        {
            throw new Exception("플레이어 랭크 DTO가 없습니다!");
        }

        PlayerRank = playerRank;
        Debug.Log($"{playerRank.Nickname} :: {playerRank.Score}점 / {playerRank.Email}등");
        

        int playerIndex = RankList.FindIndex(x => x.Nickname == PlayerRank.Nickname);
        if (playerIndex == -1)
        {
            RankList.Add(PlayerRank);
        }
        else
        {
            RankList[playerIndex] = PlayerRank;
        }
        RankList.Sort((RankDTO a, RankDTO b) => b.Score.CompareTo(a.Score));
        SaveServerData();
    }

    public RankDTO Load(string name)
    {
        PlayerRank = FindRank(name);
        return PlayerRank;
    }

    public RankDTO FindRank(string name)
    {
        return RankList.Find(x => x.Nickname == name);
    }

    private void SaveServerData()
    {
        ServerSaveData saveData = new ServerSaveData(RankList);
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_SERVER_KEY, json);
    }

    public List<RankDTO> LoadServerData()
    {
        List<RankDTO> loadedData = new List<RankDTO>();
        if (PlayerPrefs.HasKey(SAVE_SERVER_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_SERVER_KEY);
            ServerSaveData serverSaveData = JsonUtility.FromJson<ServerSaveData>(json);
            foreach (SaveData saveData in serverSaveData.RankList)
            {
                RankDTO rankDTO = new RankDTO(saveData.Score, saveData.Email, saveData.Nickname);
                loadedData.Add(rankDTO);
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                RankDTO rank = new RankDTO(i * 100, "Test#Test.com", $"Tester{i}");
                loadedData.Add(rank);
            }
        }
        return loadedData;
    }
}

[Serializable]
public class SaveData
{
    public int Score;
    public string Email;
    public string Nickname;

    public SaveData (RankDTO rankDTO)
    {
        Score = rankDTO.Score;
        Email = rankDTO.Email;
        Nickname = rankDTO.Nickname;
    }
}

[Serializable]
public class ServerSaveData
{
    public List<SaveData> RankList;

    public ServerSaveData(List<RankDTO> rankList)
    {
        RankList = new List<SaveData>();
        for (int i = 0; i < rankList.Count; i++)
        {
            SaveData saveData = new SaveData(rankList[i]);
            saveData.Email = rankList[i].Email;
            RankList.Add(saveData);
        }
    }
}

