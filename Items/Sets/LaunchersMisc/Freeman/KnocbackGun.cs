using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.LaunchersMisc.Freeman
{
	public class KnocbackGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Freeman");
			Tooltip.SetDefault("Converts rockets fired into coiled rockets that can be controlled by the cursor\n'The right man in the wrong place can make all the difference in the world'");
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 65;
			Item.height = 21;
			Item.useTime = 55;
			Item.useAnimation = 55;
			Item.useTurn = false;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 7;
			Item.channel = true;
			Item.value = Item.buyPrice(0, 11, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item36;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FreemanRocket>();
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Rocket;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			SoundEngine.PlaySound(SoundLoader.customSoundType, player.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/CoilRocket"));
			type = ModContent.ProjectileType<FreemanRocket>();
			return true;
		}
	}
}