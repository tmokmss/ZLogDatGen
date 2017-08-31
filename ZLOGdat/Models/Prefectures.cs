using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using ZLOGdat.Components;

namespace ZLOGdat.Models
{
    public class Prefectures
    {
        public IList<IList<Prefecture>> Areas { private set; get; }
        public IList<Prefecture> AreasFlat { private set; get; }

        public Prefectures()
        {
            Areas = new List<IList<Prefecture>>();
            AreasFlat = new List<Prefecture>
            {
                null // 0番はない
            };
            for (var i=1; i<49; i++)
            {
                AreasFlat.Add(new Prefecture(i));
            }
            for (var i = 0; i < 10; i++)
            {
                Areas.Add(new List<Prefecture>());
            }

            Areas[8].Add(AreasFlat[1]);

            Areas[7].Add(AreasFlat[2]);
            Areas[7].Add(AreasFlat[3]);
            Areas[7].Add(AreasFlat[4]);
            Areas[7].Add(AreasFlat[5]);
            Areas[7].Add(AreasFlat[6]);
            Areas[7].Add(AreasFlat[7]);

            Areas[0].Add(AreasFlat[8]);
            Areas[0].Add(AreasFlat[9]);

            Areas[1].Add(AreasFlat[10]);
            Areas[1].Add(AreasFlat[11]);
            Areas[1].Add(AreasFlat[12]);
            Areas[1].Add(AreasFlat[13]);
            Areas[1].Add(AreasFlat[14]);
            Areas[1].Add(AreasFlat[15]);
            Areas[1].Add(AreasFlat[16]);
            Areas[1].Add(AreasFlat[17]);

            Areas[2].Add(AreasFlat[18]);
            Areas[2].Add(AreasFlat[19]);
            Areas[2].Add(AreasFlat[20]);
            Areas[2].Add(AreasFlat[21]);

            Areas[3].Add(AreasFlat[22]);
            Areas[3].Add(AreasFlat[23]);
            Areas[3].Add(AreasFlat[24]);
            Areas[3].Add(AreasFlat[25]);
            Areas[3].Add(AreasFlat[26]);
            Areas[3].Add(AreasFlat[27]);

            Areas[9].Add(AreasFlat[28]);
            Areas[9].Add(AreasFlat[29]);
            Areas[9].Add(AreasFlat[30]);

            Areas[4].Add(AreasFlat[31]);
            Areas[4].Add(AreasFlat[32]);
            Areas[4].Add(AreasFlat[33]);
            Areas[4].Add(AreasFlat[34]);
            Areas[4].Add(AreasFlat[35]);

            Areas[5].Add(AreasFlat[36]);
            Areas[5].Add(AreasFlat[37]);
            Areas[5].Add(AreasFlat[38]);
            Areas[5].Add(AreasFlat[39]);

            Areas[6].Add(AreasFlat[40]);
            Areas[6].Add(AreasFlat[41]);
            Areas[6].Add(AreasFlat[42]);
            Areas[6].Add(AreasFlat[43]);
            Areas[6].Add(AreasFlat[44]);
            Areas[6].Add(AreasFlat[45]);
            Areas[6].Add(AreasFlat[46]);
            Areas[6].Add(AreasFlat[47]);
        }

        public void CopyFrom(Prefectures src)
        {
            for (var i=0; i<Areas.Count; i++)
            {
                for (var j=0; j<Areas[i].Count; j++)
                {
                    Areas[i][j].IsChecked = src.Areas[i][j].IsChecked;
                }
            }
        }

        public bool IsPrefectureChecked(int prefectureId)
        {
            return AreasFlat[prefectureId].IsChecked;
        }

        public void ToggleArea(int area, bool isChecked)
        {
            for (var i = 0; i < Areas[area].Count; i++)
            {
                Areas[area][i].IsChecked = isChecked;
            }
        }

        public ulong GetID()
        {
            ulong id = 0;
            for (var i=1; i<AreasFlat.Count; i++)
            {
                id = id << 1;
                if (AreasFlat[i].IsChecked)
                {
                    id = id | 1;
                }
            }
            return id;
        }

        public void SetID(ulong id)
        {
            for (var i=AreasFlat.Count-1; i>0; i--)
            {
                AreasFlat[i].IsChecked = (id & 1) == 1;
                id = id >> 1;
            }
        }
    }

    public class Prefecture
    {
        public bool IsChecked { get; set; } = true;
        [DisplayName("Name")]
        public string Name { get; set; }
        public int Id { get; set; }

        private static Number number = new Number();

        public Prefecture(string name)
        {
            Name = name;
        }
        
        public Prefecture(int id)
        {
            Id = id;
            Name = number.Refer(id);
        }

        public Prefecture()
        {
            Id = 1;
            Name = "";
        }
    }
}