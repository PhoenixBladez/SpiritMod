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
			Item.DamageType = DamageClass.Summon;
			Item.damage = 60;
			Item.Size = new Vector2(44, 48);
			Item.value = Item.sellPrice(0, 8, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.knockBack = 2;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<SanguineFlayerProj>();
			Item.shootSpeed = 1;
			Item.UseSound = SoundID.Item1;
			Item.channel = true;
			Item.autoReuse = true;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Projectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);
			if(proj.ModProjectile is SanguineFlayerProj flayer)
			{
				flayer.SwingTime = Item.useTime;
				flayer.SwingDistance = player.Distance(Main.MouseWorld);
			}

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
			return false;
		}

		public override float UseTimeMultiplier(Player player) => base.UseTimeMultiplier(player) * player.meleeSpeed; //Scale with melee speed buffs, like whips
	}
}