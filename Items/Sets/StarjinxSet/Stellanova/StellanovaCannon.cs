using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
    public class StellanovaCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellanova Cannon");
            Tooltip.SetDefault("Uses stars as ammo\nFires erratic starfire\nRight click to launch an explosive stellanova that draws in smaller stars\n50% chance not to consume ammo");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.useTime = 20;
			item.useAnimation = 20;
			item.width = 38;
            item.height = 6;
            item.damage = 80;
            item.shoot = ModContent.ProjectileType<StellanovaStarfire>();
            item.shootSpeed = StellanovaStarfire.MAX_SPEED;
            item.noMelee = true;
            item.useAmmo = AmmoID.FallenStar;
            item.value = Item.sellPrice(silver: 55);
            item.knockBack = 3f;
            item.ranged = true;
            item.rare = ItemRarityID.Pink;
			var sound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/StarCast"); //Wacky stuff so it doesn't break in mp
			item.UseSound = Main.dedServ ? sound : sound.WithPitchVariance(0.3f).WithVolume(0.7f);
			item.noUseGraphic = true;
			item.channel = true;
		}

		public override bool AltFunctionUse(Player player) => true;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);
		public override bool ConsumeAmmo(Player player) => Main.rand.NextBool();
        public override bool CanUseItem(Player player) => player.altFunctionUse != 2 || player.ownedProjectileCounts[ModContent.ProjectileType<BigStellanova>()] <= 0;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 direction = new Vector2(speedX, speedY);
            if (player.altFunctionUse == 2) //big stellanova
			{
				position += direction * 5;
				Projectile.NewProjectile(position, direction * 1.2f, ModContent.ProjectileType<BigStellanova>(), damage * 2, knockBack, player.whoAmI);
			}
            else //starfire
            {
				type = ModContent.ProjectileType<StellanovaStarfire>();
				float shootRotation = Main.rand.NextFloat(-0.3f, 0.3f);
				shootRotation = Math.Sign(shootRotation) * (float)Math.Pow(shootRotation, 2); //square the rotation offset to "weigh" it more towards 0

				direction = direction.RotatedBy(shootRotation);
                position += Vector2.Normalize(direction) * 50;
                player.itemRotation += shootRotation;
				position -= new Vector2(-10, 15).RotatedBy(player.itemRotation);

                Projectile proj = Projectile.NewProjectileDirect(position, direction, type, damage, knockBack, player.whoAmI);

				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);

				if(!Main.dedServ)
					for (int i = 0; i < 10; i++) //weak burst of particles in direction of movement
						ParticleHandler.SpawnParticle(new FireParticle(proj.Center + Vector2.Normalize(direction) * 20, player.velocity + Vector2.Normalize(proj.velocity).RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 8f),
							new Color(242, 240, 134), new Color(255, 88, 35), Main.rand.NextFloat(0.2f, 0.4f), 22, delegate (Particle p) { p.Velocity *= 0.92f; }));
			}
            return false;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 6);
            recipe.AddIngredient(ItemID.StarCannon, 1);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
