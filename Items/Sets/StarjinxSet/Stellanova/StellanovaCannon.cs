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
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellanova Cannon");
            Tooltip.SetDefault("Uses stars as ammo\nFires erratic starfire\nRight click to launch an explosive stellanova that draws in smaller stars\n50% chance not to consume ammo");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useTime = 20;
			Item.useAnimation = 20;
			Item.width = 38;
            Item.height = 6;
            Item.damage = 80;
            Item.shoot = ModContent.ProjectileType<StellanovaStarfire>();
            Item.shootSpeed = StellanovaStarfire.MAX_SPEED;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.FallenStar;
            Item.value = Item.sellPrice(silver: 55);
            Item.knockBack = 3f;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Pink;
			var sound = Mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/StarCast"); //Wacky stuff so it doesn't break in mp
			Item.UseSound = Main.dedServ ? sound : sound.WithPitchVariance(0.3f).WithVolume(0.7f);
			Item.noUseGraphic = true;
			Item.channel = true;
		}

		public override bool AltFunctionUse(Player player) => true;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow"), rotation, scale);
		public override bool CanConsumeAmmo(Item item, Player player) => Main.rand.NextBool();
        public override bool CanUseItem(Player player) => player.altFunctionUse != 2 || player.ownedProjectileCounts[ModContent.ProjectileType<BigStellanova>()] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            Vector2 direction = new Vector2(speedX, speedY);
            if (player.altFunctionUse == 2) //big stellanova
			{
				position += direction * 5;
				Projectile.NewProjectile(position, direction * 0.12f, ModContent.ProjectileType<BigStellanova>(), damage, knockback, player.whoAmI);
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

                Projectile proj = Projectile.NewProjectileDirect(position, direction, type, damage, knockback, player.whoAmI);

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
            var recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 6);
            recipe.AddIngredient(ItemID.StarCannon, 1);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
