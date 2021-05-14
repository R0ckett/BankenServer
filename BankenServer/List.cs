using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankenServer
{
    class List<T>
    {
        T[] saker = new T[0];
        public int Count
        {
            get { return saker.Length; }
        }
        public void LäggTill(T sak)
        {
            int längd = saker.Length;
            T[] tempArr = new T[längd + 1];
            for (int i = 0; i < längd; i++)
            {
                tempArr[i] = saker[i];
            }
            tempArr[längd] = sak;
            saker = tempArr;

        }
        public void TaBortVid(int idx)
        {
            T[] tempArr = new T[saker.Length - 1];
            for (int i = 0; i < tempArr.Length; i++)
            {
                if (i < idx)
                {
                    tempArr[i] = saker[i];
                }
                else
                {
                    tempArr[i] = saker[i + 1];
                }
            }
            saker = tempArr;
        }
        public void Rensa()
        {
            saker = new T[0];
        }
        public void SättVärde(T värde, int idx)
        {
            saker[idx] = värde;
        }
        public T FåVärde(int idx)
        {
            return saker[idx];
        }
    }
}
