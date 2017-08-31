using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZLOGdat.Components
{
    public class Number
    {
        IDictionary<string, string> num_ht = new Dictionary<string, string>();

        public Number()
        {
            num_ht["01"] = "北海道";
            num_ht["02"] = "青森県";
            num_ht["03"] = "岩手県";
            num_ht["04"] = "秋田県";
            num_ht["05"] = "山形県";
            num_ht["06"] = "宮城県";
            num_ht["07"] = "福島県";
            num_ht["08"] = "新潟県";
            num_ht["09"] = "長野県";
            num_ht["10"] = "東京都";
            num_ht["11"] = "神奈川県";
            num_ht["12"] = "千葉県";
            num_ht["13"] = "埼玉県";
            num_ht["14"] = "茨城県";
            num_ht["15"] = "栃木県";
            num_ht["16"] = "群馬県";
            num_ht["17"] = "山梨県";
            num_ht["18"] = "静岡県";
            num_ht["19"] = "岐阜県";
            num_ht["20"] = "愛知県";
            num_ht["21"] = "三重県";
            num_ht["22"] = "京都府";
            num_ht["23"] = "滋賀県";
            num_ht["24"] = "奈良県";
            num_ht["25"] = "大阪府";
            num_ht["26"] = "和歌山県";
            num_ht["27"] = "兵庫県";
            num_ht["28"] = "富山県";
            num_ht["29"] = "福井県";
            num_ht["30"] = "石川県";
            num_ht["31"] = "岡山県";
            num_ht["32"] = "島根県";
            num_ht["33"] = "山口県";
            num_ht["34"] = "鳥取県";
            num_ht["35"] = "広島県";
            num_ht["36"] = "香川県";
            num_ht["37"] = "徳島県";
            num_ht["38"] = "愛媛県";
            num_ht["39"] = "高知県";
            num_ht["40"] = "福岡県";
            num_ht["41"] = "佐賀県";
            num_ht["42"] = "長崎県";
            num_ht["43"] = "熊本県";
            num_ht["44"] = "大分県";
            num_ht["45"] = "宮崎県";
            num_ht["46"] = "鹿児島県";
            num_ht["47"] = "沖縄県";
            num_ht["48"] = "islands";
        }

        public string Refer(string num)
        {
            return num_ht[num];
        }

        public string Refer(int num)
        {
            return Refer(num.ToString("D2"));
        }
    }
}