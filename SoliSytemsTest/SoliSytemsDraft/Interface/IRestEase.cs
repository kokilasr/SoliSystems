using RestEase;
using SoliSytemsDraft.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;


namespace SoliSytemsDraft.Interface
{
    public interface IRestEase
    {
        //Age API
        [Get()]
        Task<UserDetails> GetAgeAsync([Query] string name);

        //Gender API
        [Get()]
        Task<UserDetails> GetGenderAsync([Query] string name);

        //Country ID API
        [Get()]
        Task<UserDetails> GetCountryAsync([Query] string name);
    }
}
