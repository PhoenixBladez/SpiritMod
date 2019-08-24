using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class Oaza : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oaza");
			Tooltip.SetDefault("Summons a random critter or enemy\nDoes not summon bosses or NPCs \n~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.width = 28;
			item.height = 32;
            item.rare = 4;
            item.maxStack = 1;
item.mana = 80;
item.consumable = false;
            item.useStyle = 4;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item43;
        }

        public override bool UseItem(Player player)
        {
            int p = Main.rand.Next(1, 580);
            int n = NPC.NewNPC((int)player.Center.X - 100, (int)player.Center.Y, p);
			if (Main.npc[n].friendly == true || Main.npc[n].boss == true|| Main.npc[n].lifeMax >= 12000)
			{
				Main.npc[n].active = false;
			}
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			            recipe.AddIngredient(null, "TimScroll", 1);
            recipe.AddIngredient(null, "FloranBar", 5);
									            recipe.AddIngredient(null, "GraniteChunk", 5);
	         recipe.AddIngredient(null, "MarbleChunk", 5);
			 			            recipe.AddIngredient(null, "CryoliteBar", 5);
			 			            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
