using Microsoft.Xna.Framework;
using SpiritMod.NPCs.MoonjellyEvent;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class TinyLunazoaItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Lunazoa");
			Tooltip.SetDefault("Increases in bait power at night, and even further during the Jelly Deluge");
		}

		public override void SetDefaults()
		{
			item.ranged = true;
			item.width = item.height = 32;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.useTime = item.useAnimation = 20;
			item.bait = 30 +
				((!Main.dayTime) ? 15 : 0) +
				((MyWorld.jellySky) ? 20 : 0);
			item.ammo = item.type;
			item.shoot = ModContent.ProjectileType<LunazoaProj>();
			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = true;

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<TinyLunazoa>());
			return false;
		}

		public override void UpdateInventory(Player player)
		{
			item.bait = 30 +
				((!Main.dayTime) ? 15 : 0) +
				((MyWorld.jellySky) ? 20 : 0);
		}
	}
}
