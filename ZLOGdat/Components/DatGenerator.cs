using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZLOGdat.Models;

namespace ZLOGdat.Components
{
    public class DatGenerator
    {
        public IList<string> JccList { private set; get; }
        public IList<string> JcgList { private set; get; }
        public IList<string> KuList { private set; get; }
        public IList<string> ShichoList { private set; get; }

        public string FilePath { set; get; }

        private static JarlList jarlList = new JarlList();
        private static Number nums = new Number();

        private Prefectures prefs;
        private string generatedText = "";
        private StringBuilder generatedSb = new StringBuilder();

        public DatGenerator(Prefectures prefs)
        {
            this.prefs = prefs;
            JccList = jarlList.JccList;
            JcgList = jarlList.JcgList;
            KuList = jarlList.KuList;
            ShichoList = jarlList.ShichoList;
        }

        private void Execute()
        {
            GenerateDat();
            Write2Dat(generatedText);
        }

        int jccLNum, jcgLNum, kuLNum, shichoLNum;
        // 現在の読み込み行番号を指定する変数群

        public string GenerateDat()
        {
            string[] jccLine = ReadValidLine("jcc");
            string[] jcgLine = ReadValidLine("jcg");
            string[] kuLine = ReadValidLine("ku");
            string[] shichoLine = ReadValidLine("shicho");
            string prefecture, ku;
            // 現在処理中の都道府県、区を記録しておく
            var existKu = false;
            // 現在処理中の市に区が存在するか？

            while (jccLNum < JccList.Count - 1)
            {
                prefecture = jccLine[0].Substring(0, 2);
                ku = kuLine[0].Substring(0, 4);

                if (prefs.IsPrefectureChecked(int.Parse(prefecture)))
                {
                    while (kuLine[0].Substring(0, 4) == jccLine[0] && kuLNum < KuList.Count + 1)
                    {
                        MakeBuffer(kuLine, jccLine[2], prefecture, "ku");
                        kuLine = ReadValidLine("ku");
                        existKu = true;
                    }

                    if (existKu)
                    //区を記載した場合、それらが属する市は記載の必要が無いため飛ばす
                    {
                        jccLine = ReadValidLine("jcc");
                        existKu = false;
                    }

                    while (jccLine[0] != kuLine[0].Substring(0, 4) && jccLine[0].Substring(0, 2) == prefecture && jccLNum < JccList.Count + 1)
                    {
                        MakeBuffer(jccLine, "", prefecture, "jcc");
                        jccLine = ReadValidLine("jcc");
                    }

                    while (jcgLine[0].Substring(0, 2) == prefecture && jcgLNum < JcgList.Count + 1 && jccLine[0].Substring(0, 2) != prefecture)
                    {
                        MakeBuffer(jcgLine, "", prefecture, "jcg");
                        jcgLine = ReadValidLine("jcg");
                    }
                }

                else
                {
                    if (prefecture == "01")
                    {
                        while (shichoLine[0].Length == 3)
                        {
                            MakeBuffer(shichoLine, "", prefecture, "shicho");
                            shichoLine = ReadValidLine("shicho");
                        }
                    }

                    else
                    {
                        var temparray = new string[3];
                        temparray[0] = prefecture;
                        temparray[1] = "";
                        temparray[2] = nums.Refer(prefecture);
                        MakeBuffer(temparray, "", prefecture, "shicho");
                    }

                    //次の都道府県県まで読み進める
                    while (kuLine[0].Substring(0, 2) == prefecture && kuLNum < KuList.Count + 1)
                    {
                        kuLine = ReadValidLine("ku");
                    }

                    while (jccLine[0].Substring(0, 2) == prefecture && jccLNum < JccList.Count + 1)
                    {
                        jccLine = ReadValidLine("jcc");
                    }

                    while (jcgLine[0].Substring(0, 2) == prefecture && jcgLNum < JcgList.Count + 1)
                    {
                        jcgLine = ReadValidLine("jcg");
                    }
                }
            }
            if (prefs.IsPrefectureChecked(48))
            {
                prefecture = "10";
                int shichoNum;
                Int32.TryParse(shichoLine[0], out shichoNum);
                while (shichoNum > 0)
                {
                    if (shichoNum < 100)
                        MakeBuffer(shichoLine, "", prefecture, "shicho");
                    shichoLine = ReadValidLine("shicho");
                    Int32.TryParse(shichoLine[0], out shichoNum);   //ダメなら0が返される
                }
            }
            generatedText = generatedSb.ToString();
            return generatedText;
        }

        private void MakeBuffer(string[] data, string shi, string prefectureNum, string type)
        {
            generatedSb.Append(data[0]);
            var indentLength = 11;
            for (var i = 0; i < indentLength - ByteCount(data[0]); i++)
            {
                generatedSb.Append(" ");
            }

            switch (type)
            {
                case "jcc":
                    if (data[0].Length == 6)
                    //東京都23区に対応する
                    {
                        generatedSb.Append(data[2] + "区\r\n");
                        break;
                    }
                    generatedSb.Append(data[2] + "市\r\n");
                    break;

                case "jcg":
                    if (data[2].Contains("支庁"))
                        generatedSb.Append(data[2] + "\r\n");
                    else
                        generatedSb.Append(data[2] + "郡\r\n");
                    break;

                case "ku":
                    generatedSb.Append(shi + "市" + data[2] + "区\r\n");
                    break;

                case "shicho":
                    generatedSb.Append(data[2] + "\r\n");
                    break;
            }
        }

        private int ByteCount(string str)
        {
            return Encoding.GetEncoding("shift-jis").GetByteCount(str);
        }

        private void Write2Dat(string buffer)
        {
            Encoding enc = Encoding.GetEncoding("shift-jis");
            using (var sw = new StreamWriter(FilePath, false, enc))
            {
                buffer = FilePath.ToUpper() + " for ZLOGCG.EXE\r\n" + buffer;
                buffer += "end of file " + FilePath;

                sw.Write(buffer);
            }
        }

        private string[] ReadValidLine(string type)
        {

            string[] jccLine = { "", "", "" }, jcgLine = { "", "", "" }, kuLine = { "", "", "" }, shichoLine = { "", "", "" }, dataArray = { "", "", "" };
            string tempstr = "";
            JarlTextParser parser = new JarlTextParser();
            switch (type)
            {
                case "jcc":
                    while (IsInvalid(tempstr))
                    {
                        if (jccLNum >= JccList.Count)
                            break;

                        if (jccLNum < JccList.Count)
                            tempstr = JccList[jccLNum].Trim();

                        jccLNum++;

                        if (!IsInvalid(tempstr))
                        {
                            jccLine = parser.Parse(tempstr);
                            if (!IsValidArray(jccLine))
                                // 無効な行ではないが、行自体が無意味である場合、次の行へ進む
                                tempstr = "";
                        }
                    }
                    dataArray = jccLine;
                    break;

                case "jcg":
                    while (IsInvalid(tempstr))
                    {
                        if (jcgLNum >= JcgList.Count)
                            break;

                        if (jcgLNum < JcgList.Count)
                            tempstr = JcgList[jcgLNum].Trim();

                        jcgLNum++;

                        if (!IsInvalid(tempstr))
                        {
                            jcgLine = parser.Parse(tempstr);
                            if (!IsValidArray(jcgLine))
                                // 無効な行ではないが、行自体が無意味である場合、次の行へ進む
                                tempstr = "";
                        }
                    }
                    dataArray = jcgLine;
                    break;

                case "ku":
                    // 区リスト1行配列の作成プロセス
                    // kuLine[0]に区ナンバー、[1]に区名の読み、[2]に区名が入る。
                    while (IsInvalid(tempstr))
                    {
                        if (kuLNum >= KuList.Count)
                            break;

                        if (kuLNum < KuList.Count)
                            tempstr = KuList[kuLNum].Trim();

                        kuLNum++;

                        if (!IsInvalid(tempstr))
                        {
                            kuLine = parser.Parse(tempstr);
                            if (!IsValidArray(kuLine))
                                // 無効な行ではないが、行自体が無意味である場合、次の行へ進む
                                tempstr = "";
                        }
                    }
                    dataArray = kuLine;
                    break;

                case "shicho":
                    //北海道の支庁、小笠原、沖ノ鳥島、南鳥島のみ記録
                    while (IsInvalid(tempstr))
                    {
                        if (shichoLNum >= ShichoList.Count)
                            break;

                        if (shichoLNum < ShichoList.Count)
                            tempstr = ShichoList[shichoLNum].Trim();

                        shichoLNum++;

                        if (!IsInvalid(tempstr))
                        {
                            shichoLine = parser.Parse(tempstr);
                            if (!IsValidArray(shichoLine))
                                tempstr = "";
                        }
                    }
                    dataArray = shichoLine;
                    break;

            }
            if (dataArray[0] == "")
                dataArray[0] = "      ";
            return dataArray;
        }

        private bool IsInvalid(string data)
        {
            if (data.Contains("※") || data.Contains("*") || data == "")
                // 区の無効、市郡の無効、初回判定時の無効
                return true;
            else
                return false;
        }

        private bool IsValidArray(string[] data)
        // 配列の1要素が全て数字ならVALID
        {
            string pattern = @"\d+";
            Regex determinator = new Regex(pattern);
            string match = determinator.Match(data[0]).ToString();
            if (match == data[0])
                return true;
            else
                return false;

        }
    }
}