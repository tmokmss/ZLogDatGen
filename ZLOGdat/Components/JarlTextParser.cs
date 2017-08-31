using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZLOGdat.Components
{
    public class JarlTextParser
    {
        public string[] Parse(string data)
        {

            var dataArray = new string[3];
            var buffer = "";
            var index = 0;
            var isSpaceBefore = true;

            for (var i = 0; i < data.Length; i++)
            {
                string letter = data.Substring(i, 1);
                if (letter == " " || letter == "　" || letter == "\t")
                {
                    if (!isSpaceBefore) //Spaceの連続はスルー
                    {
                        if (index < dataArray.Length)
                        {
                            dataArray[index] = buffer;
                            index++;
                            buffer = "";
                        }
                    }

                    isSpaceBefore = true;
                }
                else
                {
                    buffer += letter;
                    isSpaceBefore = false;
                }
            }
            if (index < dataArray.Length)   //最後にはスペースが無いため手動でバッファをフラッシュする
                dataArray[index] = buffer;

            return dataArray;
        }
    }
}