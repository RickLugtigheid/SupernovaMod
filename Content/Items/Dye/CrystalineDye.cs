using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Dye
{
    public class CrystalineDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            // Item.dye will already be assigned to this item prior to SetDefaults because of the above GameShaders.Armor.BindShader code in Load().
            // This code here remembers Item.dye so that information isn't lost during CloneDefaults.
            int dye = Item.dye;

            Item.CloneDefaults(ItemID.RedDye); // Makes the item copy the attributes of the item "Red Dye" Change "RedDye" to whatever dye type you want.

            Item.dye = dye;
        }
    }
}
