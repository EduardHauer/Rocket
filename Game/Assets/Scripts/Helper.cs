using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class Helper
    {
        public static int IDByTag(this List<ObjectPooler.Pool> self, string tag)
        {
            int result = -1;

            for (int i = 0; i < self.Count; i++)
                if (self[i].tag == tag)
                {
                    result = i;
                    break;
                }

            return result;
        }
    }
}
