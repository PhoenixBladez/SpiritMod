using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.NPCs.Boss.Dusking;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable
{
	public class DuskCrown : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Crown");
			Tooltip.SetDefault("Use at nighttime to summon Dusking");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 20;

			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;

			Item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<Dusking>()) && !Main.dayTime;

		public override bool? UseItem(Player player)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Dusking>());

			else if (Main.netMode == NetmodeID.MultiplayerClient && player == Main.LocalPlayer) {
				Vector2 spawnPos = player.Center;
				int tries = 0;
				int maxtries = 300;
				while ((Vector2.Distance(spawnPos, player.Center) <= 200 || WorldGen.SolidTile((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile2((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile3((int)spawnPos.X / 16, (int)spawnPos.Y / 16)) && tries <= maxtries) {
					spawnPos = player.Center + Main.rand.NextVector2Circular(800, 800);
					tries++;
				}

				if (tries >= maxtries)
					return false;

				SpiritMultiplayer.SpawnBossFromClient((byte)player.whoAmI, ModContent.NPCType<Dusking>(), (int)spawnPos.X, (int)spawnPos.Y);
			}

			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 3);
			recipe.AddIngredient(ItemID.SoulofNight, 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
