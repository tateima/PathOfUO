using Server.Mobiles;
using Server.Talent;

namespace Server.Items
{
    [Serializable(0, false)]
    public partial class BaseDevice : BaseWand
    {
        [Constructible]
        public BaseDevice(WandEffect wandEffect, int minCharges, int maxCharges) : base(wandEffect, minCharges, maxCharges)
        {
            Charges = 10;
            if (Parent is PlayerMobile player)
            {
                BaseTalent talent = player.GetTalent(typeof(Inventive));
                if (talent != null)
                {
                    Charges += talent.Level * 2;
                }
            }
        }
    }
}
