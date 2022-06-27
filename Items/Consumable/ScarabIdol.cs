using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Boss.Scarabeus;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class ScarabIdol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Idol");
			Tooltip.SetDefault("Non-Consumable\nUse in the desert at daytime to summon Scarabeus");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 1;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 20;

			Item.noMelee = true;
			Item.consumable = false;
			Item.autoReuse = false;

			Item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<Scarabeus>()) && player.ZoneDesert && Main.dayTime;

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Scarabeus>());
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

				SpiritMultiplayer.SpawnBossFromClient((byte)player.whoAmI, ModContent.NPCType<Scarabeus>(), (int)spawnPos.X, (int)spawnPos.Y);
			}
			SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1").WithVolume(0.3f), player.position);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Amber, 3);
			recipe.AddIngredient(ItemID.AntlionMandible, 6);
			recipe.AddRecipeGroup("SpiritMod:GoldBars", 12);
            recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
