using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Scripting;

namespace Unity.Entities
{
    [UpdateBefore(typeof(Initialization))]
    [Preserve]
    [UnityEngine.ExecuteAlways]
    public class EndFrameBarrier : BarrierSystem
    {
    }
}
