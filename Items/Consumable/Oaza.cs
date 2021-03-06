using SpiritMod.Items.Accessory;
using SpiritMod.Items.Material;
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
			Tooltip.SetDefault("Summons a random critter or enemy\nDoes not summon bosses or NPCs");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 32;
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 1;
			item.mana = 80;
			item.consumable = false;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool UseItem(Player player)
		{
			int p = Main.rand.Next(1, 580);
			int n = NPC.NewNPC((int)player.Center.X - 100, (int)player.Center.Y, p);
			if (Main.npc[n].friendly == true || Main.npc[n].boss == true || Main.npc[n].lifeMax >= 2000 || Main.npc[n].type == NPCID.MartianProbe) {
				Main.npc[n].active = false;
			}
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TimScroll>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 5);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 5);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 5);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 5);
			recipe.AddIngredient(ItemID.MeteoriteBar, 5);
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
