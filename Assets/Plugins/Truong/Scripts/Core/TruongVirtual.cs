using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public abstract class TruongVirtual
{
    public static int GetRandomIndexInList(int listCount)
    {
        return Random.Range(0, listCount);
    }

    public static int CreateRandomId()
    {
        return Random.Range(100000, 1000000);
    }

    public static double ConvertToUnixTime(DateTime time)
    {
        DateTime epoch = new System.DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        return (time - epoch).TotalSeconds;
    }

    public static DateTime ConvertFromUnixTime(double timeStamp)
    {
        DateTime epoch = new System.DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        DateTime time = epoch.AddSeconds(timeStamp);
        return time;
    }

    public static int GetHours(float totalSeconds)
    {
        return (int)(totalSeconds / 3600f);
    }

    public static int GetMinutes(float totalSeconds)
    {
        return (int)((totalSeconds / 60) % 60);
    }

    public static int GetSeconds(float totalSeconds)
    {
        return (int)(totalSeconds % 60);
    }

    public static string FormatTime(int timeMinute)
    {
        int hour = timeMinute / 60;
        int minute = timeMinute % 60;
        if (hour == 0)
        {
            return String.Format("{0:00}:{1:00}", minute, 0);
        }

        return String.Format("{0:0}:{1:00}:{2:00}", hour, minute, 0);
    }

    public static string FormatTimeSecond(int timeSecond)
    {
        int hour = timeSecond / 3600;
        int minute = (timeSecond / 60) % 60;
        int second = timeSecond % 60;
        if (hour == 0)
        {
            return String.Format("{0:00}:{1:00}", minute, second);
        }

        return String.Format("{0:0}:{1:00}:{2:00}", hour, minute, second);
    }

    #region Cryptography

    public static string XOROperator(string input, string key)
    {
        char[] output = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
            output[i] = (char)(input[i] ^ key[i % key.Length]);
        return new string(output);
    }

    public static string GenerateSHA256NonceFromRawNonce(string rawNonce)
    {
        var sha = new SHA256Managed();
        var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
        var hash = sha.ComputeHash(utf8RawNonce);

        var result = string.Empty;
        for (var i = 0; i < hash.Length; i++)
        {
            result += hash[i].ToString("x2");
        }

        return result;
    }

    public static string GenerateRandomString(int length)
    {
        if (length <= 0)
        {
            throw new Exception("Expected nonce to have positive length");
        }

        const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
        var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
        var result = string.Empty;
        var remainingLength = length;
        var randomNumberHolder = new byte[1];
        while (remainingLength > 0)
        {
            var randomNumbers = new List<int>(16);
            for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++)
            {
                cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
                randomNumbers.Add(randomNumberHolder[0]);
            }

            for (var randomNumberIndex = 0; randomNumberIndex < randomNumbers.Count; randomNumberIndex++)
            {
                if (remainingLength == 0)
                    break;
                var randomNumber = randomNumbers[randomNumberIndex];
                if (randomNumber < charset.Length)
                {
                    result += charset[randomNumber];
                    remainingLength--;
                }
            }
        }

        return result;
    }

    #endregion

    #region Other

    public static String ConvertEnumToString(Enum e)
    {
        return Enum.GetName(e.GetType(), e);
    }

    public static TEnumType ConvertStringToEnum<TEnumType>(String str)
    {
        return (TEnumType)Enum.Parse(typeof(TEnumType), str);
    }

    public static void ShuffleList<T>(object objList)
    {
        var list = (List<T>)objList;
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count - 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    #endregion

    public static void PlayParticleSystem(GameObject targetGo)
    {
        var list = targetGo.GetComponentsInChildren<ParticleSystem>();
        if (list == null) return;
        {
            if (list.Length == 0) return;
            foreach (var item in list)
                item.Play();
        }
    }

    public static void StopParticleSystem(GameObject targetGo)
    {
        var list = targetGo.GetComponentsInChildren<ParticleSystem>();
        if (list == null) return;
        {
            if (list.Length == 0) return;
            foreach (var item in list)
                item.Stop();
        }
    }

    public static string GetNumbersFromString(string input)
    {
        string pattern = @"\d+";
        string numbers = "";

        foreach (Match match in Regex.Matches(input, pattern))
        {
            numbers += match.Value;
        }

        return numbers;
    }

    /// <summary>
    /// ex: number = 25
    /// calculate 25 = 5*5
    /// result = 5
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetSquareRoot(int number)
    {
        for (int i = 1; i <= Math.Sqrt(number); i++)
        {
            if (i * i == number)
            {
                return i;
            }
        }

        return -1;
    }

    public static void SetNameObject(Transform obj, string prefabName)
    {
        obj.name = prefabName;
    }

    public static void AddIdToObject(int id, Transform obj)
    {
        if (obj.GetComponent<TruongId>() != null) return;
        var c = obj.gameObject.AddComponent<TruongId>();
        c.SetId(id);
    }

    public static bool IsUnityEditor()
    {
        return Application.platform == RuntimePlatform.WindowsEditor;
    }

    public static string GetColorTextFromHex(float content, string colorHex)
    {
        return GetColorTextFromHex(content.ToString(CultureInfo.InvariantCulture), colorHex);
    }

    public static string GetColorTextFromHex(string content, string colorHex)
    {
        return "<color=#" + colorHex + ">" + content + "</color >";
    }


    public static List<T> GetListItemFromEnum<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    public static List<string> LoadListNameOfListSprite(List<Sprite> sprites)
    {
        List<string> result = new List<string>();
        sprites.ForEach(item => result.Add(item.name));
        return result;
    }

    public static List<string> GetNamesEnum<T>()
    {
        return Enum.GetNames(typeof(T)).ToList();
    }

    public static int GetTimeAgo_Minute(string inputTime)
    {
        System.DateTime targetTime = System.DateTime.ParseExact(inputTime, "yyyy-MM-dd HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture);
        System.TimeSpan timeDifference = System.DateTime.Now - targetTime;
        int minutesDifference = (int)timeDifference.TotalMinutes;
        return minutesDifference;
    }
}