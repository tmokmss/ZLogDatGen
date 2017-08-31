using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace ZLOGdat.Components
{
    public class JarlList
    {
        private string jccListURL = "http://www.jarl.org/Japanese/A_Shiryo/A-2_jcc-jcg/jcc-list.txt";
        private string jcgListURL = "http://www.jarl.org/Japanese/A_Shiryo/A-2_jcc-jcg/jcg-list.txt";
        private string kuListURL = "http://www.jarl.org/Japanese/A_Shiryo/A-2_jcc-jcg/ku-list.txt";
        private string shichoURL = "http://www.jarl.org/Japanese/A_Shiryo/A-2_jcc-jcg/shicho-list.txt";

        public IList<string> JccList { private set; get; }
        public IList<string> JcgList { private set; get; }
        public IList<string> KuList { private set; get; }
        public IList<string> ShichoList { private set; get; }

        public JarlList()
        {
            GetLists();
        }

        private void GetLists()
        {
            WebClient wc = new WebClient();
            Stream st;
            StreamReader sr;

            st = wc.OpenRead(jccListURL);
            sr = new StreamReader(st, Encoding.GetEncoding("shift-jis"));
            JccList = SplitIntoLines(sr);

            st = wc.OpenRead(jcgListURL);
            sr = new StreamReader(st, Encoding.GetEncoding("shift-jis"));
            JcgList = SplitIntoLines(sr);

            st = wc.OpenRead(kuListURL);
            sr = new StreamReader(st, Encoding.GetEncoding("shift-jis"));
            KuList = SplitIntoLines(sr);

            st = wc.OpenRead(shichoURL);
            sr = new StreamReader(st, Encoding.GetEncoding("shift-jis"));
            ShichoList = SplitIntoLines(sr);

            sr.Close();
            st.Close();
        }

        private IList<string> SplitIntoLines(StreamReader sr)
        {
            // ちょっとずつ読んでいく方法だと、途中で切れることがあったので、最初に一気に読む
            var str = sr.ReadToEnd();
            var strarray = str.Split('\n');
            var list = new List<string>(strarray);
            return list;
        }
    }
}