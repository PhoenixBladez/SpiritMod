using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;
using SpiritMod.Prim;
using SpiritMod.VerletChains;
using System.Collections.Generic;

namespace SpiritMod.Items.Sets.OlympiumSet.BetrayersChains
{
	public class BetrayersChains : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blades of Chaos");
			// Tooltip.SetDefault("Plugs into tiles, changing the chain into a shocking livewire");

		}

		public override void SetDefaults() {
            item.width = 16;
            item.height = 16;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 60;
            item.useTime = 60;
            item.shootSpeed = 4f;
            item.knockBack = 4f;
            item.UseSound = SoundID.Item116;
            item.shoot = ModContent.ProjectileType<BetrayersChainsProj>();
            item.value = Item.sellPrice(gold: 2);
            item.noMelee = true;
            item.noUseGraphic = true;
            item.channel = true;
            item.autoReuse = true;
            item.melee = true;
            item.damage = 50;
            item.rare = ItemRarityID.LightRed;
        }
        int combo;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            // How far out the inaccuracy of the shot chain can be.
            float radius = 2f;
            // Sets ai[1] to the following value to determine the firing direction.
            // The smaller the value of NextFloat(), the more accurate the shot will be. The larger, the less accurate. This changes depending on your radius.
            // NextBool().ToDirectionInt() will have a 50% chance to make it negative instead of positive.
            // The Solar Eruption uses this calculation: Main.rand.NextFloat(0f, 0.5f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(45f);
            float offset = Main.rand.NextFloat(0.25f, 1f);
            bool directionbool = Main.rand.NextBool();
            float direction = offset * directionbool.ToDirectionInt() * radius;
            Projectile projectile = Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0f, direction);
            // Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
            if (projectile.modProjectile is BetrayersChainsProj modItem)
            {
                switch (combo % 3)
                {
                    case 0:
                            modItem.firingSpeed = item.shootSpeed * 3f * Main.rand.NextFloat(1, 1.5f);
                            modItem.firingAnimation = item.useAnimation * 0.66f;
                            modItem.firingTime = item.useTime * 0.66f;
                            modItem.InitializeChain(player.MountedCenter);
                            break;
                    case 1:
                            modItem.firingSpeed = item.shootSpeed * 3f * Main.rand.NextFloat(1, 1.5f);
                            modItem.firingAnimation = item.useAnimation * 0.66f;
                            modItem.firingTime = item.useTime * 0.66f;
                            modItem.InitializeChain(player.MountedCenter);
                            break;
                    case 2:
                            modItem.firingSpeed = item.shootSpeed * 2.6f * Main.rand.NextFloat(1, 1.5f);
                            modItem.firingAnimation = item.useAnimation;
                            modItem.firingTime = item.useTime;
                            modItem.combo = true;
                            modItem.InitializeChain(player.MountedCenter);
                            direction = offset * (!directionbool).ToDirectionInt() * radius;
                            projectile = Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0f, direction);
                            // Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
                            if (projectile.modProjectile is BetrayersChainsProj modItem2)
                            {
                                modItem2.firingSpeed = item.shootSpeed * 2.6f * Main.rand.NextFloat(1, 1.5f);
                                modItem2.firingAnimation = item.useAnimation;
                                modItem2.firingTime = item.useTime;
                                modItem2.combo = true;
                                modItem2.InitializeChain(player.MountedCenter);
                            }
                            break;
                }
            }
            combo++;
            return false;
        }
	}
	public class BetrayersChainsProj : ModProjectile
	{
		public Vector2 chainHeadPosition;
        public float firingSpeed;
        public float firingAnimation;
        public float firingTime;
        public bool combo = false;
        public Chain chain;
        public Vector2 spawnPos;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Blades of Chaos");
        }

        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 1;
            projectile.ownerHitCheck = true;
        }

		public void InitializeChain(Vector2 position) => chain = new Chain(8, 16, position, new ChainPhysics(0.95f, 0.5f, 0.4f));

		// This projectile uses advanced calculation for its motion.
		bool primsCreated = false;
        public override void AI() {
            Player player = Main.player[projectile.owner];
            if (chainHeadPosition == Vector2.Zero)
            {
                chainHeadPosition = player.Center;
            }
            if (!primsCreated && combo)
            {
                primsCreated = true;
                SpiritMod.primitives.CreateTrail(new FireChainPrimTrail(projectile));
            }
            // Face the projectile towards its movement direction, offset by 90 degrees counterclockwise because the sprite faces downward.
            projectile.rotation = projectile.velocity.ToRotation() -1.57f;

            // Constantly set the chain's timeLeft to 2 so that it doesn't die.
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            if (combo)
            {
                player.itemTime = 15;
                player.itemAnimation = 15;
            }
            else
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

            // Use one of the projectile's localAI slot as a cooldown timer for spawning explosions. When an explosion is spawned, this gets set to 4, so it takes 4 ticks to reach 0 again.
            if (projectile.localAI[1] > 0f)
                projectile.localAI[1] -= 1f;

            // The projectile's swerving motion.
            
            // If this localAI slot is 0, meaning it doesn't have an assigned value, then set it to the projectile's rotation so that we can get the rotation it had on its first tick of being spawned.
            if (projectile.localAI[0] == 0f)
                projectile.localAI[0] = projectile.rotation;

            // If localAI[0] (the localAI slot we use to store initial rotation)'s X value is greater than 0, then direction is 1. Otherwise, -1.
            float direction = (projectile.localAI[0].ToRotationVector2().X >= 0f).ToDirectionInt();

            // Use a sine calculation to rotate the Solar Eruption around to form an ovular motion.
            Vector2 rotation = (direction * (projectile.ai[0] / firingAnimation * MathHelper.ToRadians(360f) + MathHelper.ToRadians(-90f))).ToRotationVector2();
            rotation.Y *= (float)Math.Sin(projectile.ai[1]);

            rotation = rotation.RotatedBy(projectile.localAI[0]);

            // Use the ai[0] slot as a timer to increment how long the projectile has been alive.
            projectile.ai[0] += 1f;
            if (projectile.ai[0] < firingTime) {
                projectile.velocity += (firingSpeed * rotation).RotatedBy(MathHelper.ToRadians(90f));
            }
            else {
                // If past the firingTime variable we set in the item's Shoot() hook, kill it.
                projectile.Kill();
            }

            // Manages the positioning for the chain's handle.
            Vector2 offset = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;

            // Flip the offset horizontally if the player is facing left instead of right.
            if (player.direction == -1) {
                offset.X = player.bodyFrame.Width - offset.X;
            }
            // Flip the offset vetically if the player is using gravity (such as a Gravity Globe or Gravitation Potion.)
            if (player.gravDir == -1f) {
                offset.Y = player.bodyFrame.Height - offset.Y;
            }
            // This line is a custom offset that you can change to move the handle around. Default is 0f, 0f. This projectile uses 4f, -6f.
            offset += new Vector2(4f, -6f) * new Vector2(player.direction, player.gravDir);
            offset -= new Vector2(player.bodyFrame.Width - projectile.width, player.bodyFrame.Height - 42) * 0.5f;
            projectile.Center = player.RotatedRelativePoint(player.position + offset) - projectile.velocity;

            chain.Update(player.MountedCenter - new Vector2(0,1), chainHeadPosition);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            int cooldown = 10;
            projectile.localNPCImmunity[target.whoAmI] = 20;
            target.immune[projectile.owner] = cooldown;
            if (combo)
				target.AddBuff(BuffID.OnFire, 180);
        }

        // Set to true so the projectile can break tiles like grass, pots, vines, etc.
        public override bool? CanCutTiles() => true;

        // Plot a line from the start of the Solar Eruption to the end of it, to change the tile-cutting collision logic. (Don't change this.)
        public override void CutTiles() {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity, (projectile.width + projectile.height) * 0.5f * projectile.scale, DelegateMethods.CutTiles);
        }

        // Plot a line from the start of the Solar Eruption to the end of it, and check if any hitboxes are intersected by it for the entity collision logic. (Don't change this.)
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            // Custom collision so all chains across the flail can cause impact.
            float collisionPoint = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity, (projectile.width + projectile.height) * 0.5f * projectile.scale, ref collisionPoint)) {
                return true;
            }
            return false;
        }
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
            chain.Draw(spriteBatch, ModContent.GetTexture(Texture + "_Chain"), Main.projectileTexture[projectile.type]);

			return false;
		}
	}
}