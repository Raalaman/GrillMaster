using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public interface IAsyncRoundGenerator<T, U>
    {
        Task<List<U>> GenerateRounds(T menu);
    }

}
