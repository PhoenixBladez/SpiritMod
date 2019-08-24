using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon
{
    public class Tesseract : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Tesseract");
            Tooltip.SetDefault("Eliminates all non boss enemies on the screen\nConsumes 1/10 of the user's life on use");

        }


        public override void SetDefaults()
        {
            item.damage = 0;
            item.magic = true;
            item.mana = 200;
            item.width = 42;
            item.height = 56;
            item.useTime = 80;
            item.useAnimation = 80;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.buyPrice(0, 90, 0, 0);
            item.rare = 10;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
        }
        public override bool CanUseItem(Player player)
        {
            //return NPC.downedBoss3;
            return player.statLife >= (player.statLifeMax2 / 4);
        }

        public override bool UseItem(Player player)
        {
            for (int npcFinder = 0; npcFinder < 200; ++npcFinder)
            {
                if (!Main.npc[npcFinder].boss && !Main.npc[npcFinder].townNPC && Main.npc[npcFinder].lifeMax <= 3000)
                {
                    Main.npc[npcFinder].life = 0;
                }

            }
            player.statLife -= player.statLifeMax2 / 10;
            return true;
        }
    }
}
