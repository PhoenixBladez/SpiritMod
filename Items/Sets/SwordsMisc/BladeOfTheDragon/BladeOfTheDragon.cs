using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.SwordsMisc.BladeOfTheDragon
{
    public class BladeOfTheDragon : ModItem
    {

		public int combo;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of the Dragon");
            Tooltip.SetDefault("Hold and release to slice through nearby enemies");
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 80;
            item.width = 60;
            item.height = 60;
            item.useTime = 60;
            item.useAnimation = 60;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 1;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 90, 0);
            item.rare = ItemRarityID.Orange;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("BladeOfTheDragonProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }
		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
			var modProj = proj.modProjectile as BladeOfTheDragonProj;
			if (combo % 4 != 0)
				modProj.charge = 39;
			return false;
		}
	}
    public class BladeOfTheDragonProj : ModProjectile
    {
        public NPC[] hit = new NPC[24];

		Vector2 direction = Vector2.Zero;
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blade of the Dragon");

		public override void SetDefaults()
		{
            projectile.width = projectile.height = 40;
			projectile.hostile = false;
			projectile.melee = true;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
            projectile.alpha = 255;
			projectile.timeLeft = 240;
		}

        public readonly int MAXCHARGE = 50;
        public int charge = 0;
        int index = 0;
        NPC mostrecent;

		bool comboActivated = false;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.heldProj = projectile.whoAmI;
            projectile.Center = player.Center;
            if (player.channel && projectile.timeLeft > 237)
            {
				player.itemTime = 5;
				player.itemAnimation = 5;
				projectile.timeLeft = 240;
                charge++;
                if (charge < 40)
                    charge++;
                if (charge == 40)
                {
					direction = Main.MouseWorld - (player.Center);
					direction.Normalize();
					direction *= 60f;
					Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/slashdash").WithPitchVariance(0.4f).WithVolume(0.4f), projectile.Center);
					SpiritMod.primitives.CreateTrail(new DragonPrimTrail(projectile));
                }
                if (charge > 40 && charge < MAXCHARGE)
                {
					player.GetModPlayer<MyPlayer>().AnimeSword = true;
                    player.velocity = direction;
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        NPC target = Main.npc[i];
                        if (Collision.CheckAABBvAABBCollision(target.position, new Vector2(target.width, target.height), player.position - new Vector2(10, 0), new Vector2(player.width + 20, player.height)) && index < 23)
                        {
							if (player.HeldItem.modItem is BladeOfTheDragon modItem && !comboActivated)
							{
								comboActivated = true;
								modItem.combo++;
							}
                            bool inlist = false;
                            foreach (var npc in hit)
				                if (target == npc)
                                    inlist = true;
                            if (!inlist)
                                hit[index++] = target;
                        }
                    }
                }
                if (charge == MAXCHARGE)
                {
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
					player.velocity = Vector2.Zero;
                }
            }
            else
            {
                if (charge > 40 && charge < MAXCHARGE)
                {
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
					player.velocity = Vector2.Zero;
                    charge = MAXCHARGE + 1;
                }
                if (projectile.timeLeft % 3 == 0)
                {
                    float mindist = 0;
                    NPC closest = null;
                    foreach (var npc in hit)
                    {
                        if (npc != null)
                        {
                            if (npc.active && (!npc.townNPC || !npc.friendly))
                            {
                                float distance = (npc.Center - projectile.Center).Length();
                                if (mostrecent == null)
                                {
                                    if (distance > mindist)
                                    {
                                        closest = npc;
                                        mindist = distance;
                                    }
                                }
                                else
                                {
                                    float maxdistance = (mostrecent.Center - projectile.Center).Length();
                                    if (distance > mindist && distance < maxdistance)
                                    {
                                        closest = npc;
                                        mindist = distance;
                                    }
                                }
                            }
                        }
                    }
					if (closest != null)
					{
						mostrecent = closest;
						if (mostrecent.active)
							SpiritMod.primitives.CreateTrail(new DragonPrimTrailTwo(mostrecent));
					}
					else if (projectile.timeLeft > 15)
					{
						if (player.HeldItem.modItem is BladeOfTheDragon modItem && !comboActivated)
						{
							modItem.combo = 0;
						}
						projectile.timeLeft = 15;
					}
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            if (charge > 40 && charge < MAXCHARGE)
                return base.Colliding(projHitbox, targetHitbox);
            if (!(player.channel && projectile.timeLeft > 237))
                return true;
            return false;

        }

        public override bool? CanHitNPC(NPC target)
		{
            Player player = Main.player[projectile.owner];
            if ((player.channel && projectile.timeLeft > 237) || projectile.timeLeft > 5)
                return false;
            foreach (var npc in hit)
				if (target == npc)
					return base.CanHitNPC(target);
			return false;
        }

		public override void Kill(int timeLeft) => Main.player[projectile.owner].GetModPlayer<MyPlayer>().AnimeSword = false;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
            Texture2D texture = Main.projectileTexture[projectile.type];
			if (player.channel && projectile.timeLeft > 237)
			{
				if (player.direction == 1)
				{
					Main.spriteBatch.Draw(texture, (player.MountedCenter + new Vector2(6, 6)) - Main.screenPosition, null, lightColor, 0, new Vector2(texture.Width * 0.75f, texture.Height * 0.2f), projectile.scale, SpriteEffects.None, 0.0f);
					if (charge > 40 && player.channel)
					{
						Texture2D texture2 = ModContent.GetTexture("SpiritMod/Items/Weapon/Swung/AnimeSword/TwinkleXLarge");
						Main.spriteBatch.Draw(texture2, (player.MountedCenter + new Vector2(0, 6)) - Main.screenPosition, null, Color.White, charge / 40f, new Vector2(texture2.Width / 2, texture2.Height / 2), projectile.scale, SpriteEffects.None, 0.0f);
					}
				}
				else
				{
					Main.spriteBatch.Draw(texture, (player.MountedCenter + new Vector2(6, 6)) - Main.screenPosition, null, lightColor, 0, new Vector2(texture.Width * 0.25f, texture.Height * 0.2f), projectile.scale, SpriteEffects.FlipHorizontally, 0.0f);
					if (charge > 40 && player.channel)
					{
						Texture2D texture2 = ModContent.GetTexture("SpiritMod/Items/Weapon/Swung/AnimeSword/TwinkleXLarge");
						Main.spriteBatch.Draw(texture2, (player.MountedCenter + new Vector2(0, 6)) - Main.screenPosition, null, Color.White, charge / 40f, new Vector2(texture2.Width / 2, texture2.Height / 2), projectile.scale, SpriteEffects.None, 0.0f);
					}
				}
			}
            return false;
        }
    }
}