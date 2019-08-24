/*using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class TatteredSigil : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tattered Sigil");
			Tooltip.SetDefault("???");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 4;
            item.maxStack = 99;
            item.useStyle = 4;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }

        public override bool CanUseItem(Player player)
        {
            return Main.invasionType <= 0 && InvasionWorld.invasionType <= 0;
        }

        public override bool UseItem(Player player)
        {
            InvasionHandler.StartCustomInvasion(SpiritMod.customEvent);
            return true;
        }
    }
}*/
