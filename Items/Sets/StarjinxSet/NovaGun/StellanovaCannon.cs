using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.NovaGun
{
    public class StellanovaCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellanova Cannon");
            Tooltip.SetDefault("Fires erratic stars\nRight click to launch an explosive stellanova that draws in smaller stars\n50% chance not to consume ammo");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.useAnimation = 40;
            item.useTime = 40;
            item.width = 38;
            item.height = 6;
            item.damage = 53;
            item.shoot = ModContent.ProjectileType<NovaGunProjectile>();
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.useAmmo = AmmoID.FallenStar;
            item.value = Item.sellPrice(silver: 55);
            item.knockBack = 3f;
            item.ranged = true;
            item.rare = ItemRarityID.Pink;

			var sound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/StarCast");
			item.UseSound = Main.dedServ ? sound : sound.WithPitchVariance(0.3f).WithVolume(0.7f);
		}

		public override Vector2? HoldoutOffset() => new Vector2(-20, -10);
		public override bool AltFunctionUse(Player player) => true;

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);

		public override bool ConsumeAmmo(Player player) => Main.rand.NextBool();
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useAnimation = 40;
                item.useTime = 40;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<NovaGunStar>()] > 0)
                    return false;
            }
            else
            {
                item.useAnimation = 10;
                item.useTime = 10;
			}
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 direction = new Vector2(speedX, speedY);
            if (player.altFunctionUse == 2)
			{
				position += direction * 5;
				Projectile.NewProjectile(position, direction * 1.2f, ModContent.ProjectileType<NovaGunStar>(), damage * 2, knockBack, player.whoAmI);
				if(!Main.dedServ)
					for(int i = 0; i < 8; i++)
						ParticleHandler.SpawnParticle(new StarParticle(position, player.velocity + direction.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(0.33f, 0.8f),
							Color.White * 0.66f, SpiritMod.StarjinxColor(Main.GlobalTime + i), Main.rand.NextFloat(0.2f, 0.3f), 20));
			}
            else
            {
                float shootRotation = Main.rand.NextFloat(-0.1f, 0.1f);
                direction = direction.RotatedBy(shootRotation);
                position += direction * 5;
                player.itemRotation += shootRotation;
                Projectile.NewProjectile(position, direction, type, damage, knockBack, player.whoAmI);

				if (!Main.dedServ)
					for (int i = 0; i < 3; i++)
						ParticleHandler.SpawnParticle(new StarParticle(position, player.velocity + direction.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(0.2f, 0.5f),
							Color.White * 0.66f, SpiritMod.StarjinxColor(Main.GlobalTime + i), Main.rand.NextFloat(0.2f, 0.3f), 20));
			}
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "Starjinx", 6);
            recipe.AddIngredient(ItemID.StarCannon, 4);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
