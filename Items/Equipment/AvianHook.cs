using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Equipment
{
    internal class AvianHook : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Avian Hook");
        }

        public override void SetDefaults() {
            /*
				this.noUseGraphic = true;
				this.damage = 0;
				this.knockBack = 7f;
				this.useStyle = 5;
				this.name = "Amethyst Hook";
				this.shootSpeed = 10f;
				this.shoot = 230;
				this.width = 18;
				this.height = 28;
				this.useSound = 1;
				this.useAnimation = 20;
				this.useTime = 20;
				this.rare = 1;
				this.noMelee = true;
				this.value = 20000;
			*/
            // Instead of copying these values, we can clone and modify the ones we want to copy
            item.CloneDefaults(ItemID.AmethystHook);
            item.shootSpeed = 12f; // how quickly the hook is shot.
            item.shoot = ProjectileType<AvianHookProjectile>();
        }
    }

    internal class AvianHookProjectile : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
        }

        public override void SetDefaults() {
            /*	this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			*/
            projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            //projectile.timeLeft = 400;
        }

        // Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
        public override bool? CanUseGrapple(Player player) {
            int hooksOut = 0;
            for(int l = 0; l < 1000; l++) {
                if(Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type) {
                    hooksOut++;
                }
            }
            if(hooksOut > 1) // This hook can have 2 hooks out.
            {
                return false;
            }
            return true;
        }

        // Return true if it is like: Hook, CandyCaneHook, BatHook, GemHooks
        //public override bool? SingleGrappleHook(Player player)
        //{
        //	return true;
        //}

        // Use this to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
        // You can also change the projectile like: Dual Hook, Lunar Hook
        //public override void UseGrapple(Player player, ref int type)
        //{
        //	int hooksOut = 0;
        //	int oldestHookIndex = -1;
        //	int oldestHookTimeLeft = 100000;
        //	for (int i = 0; i < 1000; i++)
        //	{
        //		if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
        //		{
        //			hooksOut++;
        //			if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
        //			{
        //				oldestHookIndex = i;
        //				oldestHookTimeLeft = Main.projectile[i].timeLeft;
        //			}
        //		}
        //	}
        //	if (hooksOut > 1)
        //	{
        //		Main.projectile[oldestHookIndex].Kill();
        //	}
        //}

        // Amethyst Hook is 300, Static Hook is 600
        public override float GrappleRange() {
            return 400f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks) {
            numHooks = 1;
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed) {
            speed = 13f;
        }


        bool swinging = false;
        float xvel = 0;
        float yvel = 0;
        Vector2 position = Vector2.Zero;
        float momentum = 0;
        float distance = 0;
        Vector2 angle = Vector2.Zero;
        float deltaX = 0;
        float deltaY = 0;

        bool leftswing = false;
        bool rightswing = false;

        int xDist = 0;
        public override void GrapplePullSpeed(Player player, ref float speed) {
            speed = 0f;
            if(!swinging) {
                swinging = true;

                deltaX = player.position.X + (float)(player.width / 2) - projectile.Center.X;
                deltaY = player.position.Y + (float)(player.height / 2) - projectile.Center.Y;
                angle = new Vector2(deltaX, deltaY);
                angle.Normalize();
                distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
                angle *= distance;
                if(player.position.X + (float)(player.width / 2) > projectile.Center.X) {
                    rightswing = true;
                } else {
                    leftswing = true;
                }
            }
            player.position = angle + projectile.Center;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            Texture2D texture = ModContent.GetTexture("SpiritMod/Items/Equipment/AvianHookChain");
            Vector2 vector = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Rectangle? sourceRectangle = null;
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num = (float)texture.Height;
            Vector2 vector2 = mountedCenter - vector;
            float rotation = (float)Math.Atan2((double)vector2.Y, (double)vector2.X) - 1.57f;
            bool flag = true;
            if(float.IsNaN(vector.X) && float.IsNaN(vector.Y)) {
                flag = false;
            }
            if(float.IsNaN(vector2.X) && float.IsNaN(vector2.Y)) {
                flag = false;
            }
            while(flag) {
                if((double)vector2.Length() < (double)num + 1.0) {
                    flag = false;
                } else {
                    Vector2 value = vector2;
                    value.Normalize();
                    vector += value * num;
                    vector2 = mountedCenter - vector;
                    Color color = Lighting.GetColor((int)vector.X / 16, (int)((double)vector.Y / 16.0));
                    color = projectile.GetAlpha(color);
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        public override void AI() {
            int num = 5;
            for(int k = 0; k < 1; k++) {
                int index2 = Dust.NewDust(new Vector2(projectile.Center.X + 15, projectile.Center.Y), 1, 1, 180, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .5f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
            for(int j = 0; j < 1; j++) {
                int index2 = Dust.NewDust(new Vector2(projectile.Center.X - 15, projectile.Center.Y), 1, 1, 130, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)j;
                Main.dust[index2].scale = .5f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
        }
    }

    // Animated hook example
    // Multiple, 
    // only 1 connected, spawn mult
    // Light the path
    // Gem Hooks: 1 spawn only
    // Thorn: 4 spawns, 3 connected
    // Dual: 2/1 
    // Lunar: 5/4 -- Cycle hooks, more than 1 at once
    // AntiGravity -- Push player to position
    // Static -- move player with keys, don't pull to wall
    // Christmas -- light ends
    // Web slinger -- 9/8, can shoot more than 1 at once
    // Bat hook -- Fast reeling

}
