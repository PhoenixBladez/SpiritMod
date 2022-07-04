using SpiritMod.NPCs.Boss;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable
{
	public class JewelCrown : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feather Crown");
			Tooltip.SetDefault("Use in the sky to summon the Ancient Avian");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<AncientFlyer>()) && (player.ZoneOverworldHeight || player.ZoneSkyHeight);

		public override bool? UseItem(Player player)
		{
			if (player.ZoneOverworldHeight || player.ZoneSkyHeight)
			{
				if (Main.netMode == NetmodeID.SinglePlayer)
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<AncientFlyer>());

				else if (Main.netMode == NetmodeID.MultiplayerClient && player == Main.LocalPlayer)
				{
					Vector2 spawnPos = player.Center;
					int tries = 0;
					int maxtries = 300;
					while ((Vector2.Distance(spawnPos, player.Center) <= 200 || WorldGen.SolidTile((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile2((int)spawnPos.X / 16, (int)spawnPos.Y / 16) || WorldGen.SolidTile3((int)spawnPos.X / 16, (int)spawnPos.Y / 16)) && tries <= maxtries)
					{
						spawnPos = player.Center + Main.rand.NextVector2Circular(800, 800);
						tries++;
					}

					if (tries >= maxtries)
						return false;

					SpiritMultiplayer.SpawnBossFromClient((byte)player.whoAmI, ModContent.NPCType<AncientFlyer>(), (int)spawnPos.X, (int)spawnPos.Y);
				}

				SoundEngine.PlaySound(SoundID.Roar, player.position);
				return null;
			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.FallenStar, 2);
			recipe.AddIngredient(ItemID.Feather, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}