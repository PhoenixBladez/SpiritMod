
using SpiritMod.Items.Sets.SlagSet;
using SpiritMod.NPCs.Boss.Infernon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable
{
	public class CursedCloth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pain Caller");
			Tooltip.SetDefault("Use in the Underworld to summon Infernon");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<Infernon>()) && !(player.position.Y / 16f < Main.maxTilesY - 200);
		}

		public override bool UseItem(Player player)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Infernon>());

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

				SpiritMod.WriteToPacket(SpiritMod.Instance.GetPacket(), (byte)MessageType.BossSpawnFromClient, (byte)player.whoAmI, ModContent.NPCType<Infernon>(), (int)spawnPos.X, (int)spawnPos.Y).Send(-1);
			}
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofNight, 4);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 3);
			recipe.AddIngredient(ItemID.HellstoneBar, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
