using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.SanguineFlayer
{
	public class SanguineFlayerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Flayer");
			Tooltip.SetDefault("Hooks into hit enemies while the attack button is held, granting 7 summon tag damage while hooked\n" +
				"Release the attack button while hooked into an enemy to rip the weapon out\n" +
				"Summon damage on hooked enemies builds up sanguine energy, increasing the damage dealt when ripping the weapon out");
		}

		public override void SetDefaults()
		{
			item.summon = true;
			item.damage = 60;
			item.Size = new Vector2(44, 48);
			item.value = Item.sellPrice(0, 8, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.knockBack = 2;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 45;
			item.useAnimation = 45;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<SanguineFlayerProj>();
			item.shootSpeed = 1;
			item.UseSound = SoundID.Item1;
			item.channel = true;
			item.autoReuse = true;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
			if(proj.modProjectile is SanguineFlayerProj flayer)
			{
				flayer.SwingTime = item.useTime;
				flayer.SwingDistance = player.Distance(Main.MouseWorld);
			}

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);

			return false;
		}

		public override float UseTimeMultiplier(Player player) => base.UseTimeMultiplier(player) * player.meleeSpeed; //Scale with melee speed buffs, like whips
	}
}