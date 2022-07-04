using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
using SpiritMod.Items.Sets.SwordsMisc.BladeOfTheDragon;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Swung.AnimeSword
{
	public class AnimeSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anime Sword");
			Tooltip.SetDefault("Hold and release to slice through nearby enemies");
		}

		public override void SetDefaults()
		{
			Item.channel = true;
			Item.damage = 32;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.crit = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.rare = ItemRarityID.Orange;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<AnimeSwordProj>();
			Item.shootSpeed = 6f;
			Item.noUseGraphic = true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}

	public class AnimeSwordProj : ModProjectile
	{
		public NPC[] hit = new NPC[12];

		Vector2 direction = Vector2.Zero;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Anime Sword Proj");

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 40;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public readonly int MAXCHARGE = 69;
		public int charge = 0;
		public int postCharge = 0;
		int index = 0;
		NPC mostRecent;

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			Projectile.Center = player.Center;

			if (player.channel)
			{
				Projectile.timeLeft = 120;
				charge++;
				if (charge < 60)
					charge++;

				if (charge == 60)
				{
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/slashdash") with { PitchVariance = 0.4f, Volume = 0.4f }, Projectile.Center);
					SpiritMod.primitives.CreateTrail(new AnimePrimTrail(Projectile));
					if (Projectile.owner == Main.myPlayer)
					{
						direction = Vector2.Normalize(Main.MouseWorld - player.Center) * 45f;

						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
					}
				}

				if (charge > 60 && charge < MAXCHARGE)
				{
					player.GetModPlayer<MyPlayer>().AnimeSword = true;
					player.GetModPlayer<DragonPlayer>().DrawSparkle = true;
					player.velocity = direction;
					player.direction = System.Math.Sign(player.velocity.X);

					for (int i = 0; i < Main.npc.Length; i++)
					{
						NPC target = Main.npc[i];
						if (Collision.CheckAABBvAABBCollision(target.position, new Vector2(target.width, target.height), player.position - new Vector2(10, 0), new Vector2(player.width + 20, player.height)) && index < 11)
						{
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
					player.velocity *= 0.001f;

					player.channel = false;
				}
			}
			else
			{
				if (charge > 60 && charge < MAXCHARGE)
				{
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
					player.velocity = Vector2.Zero;
					charge = MAXCHARGE + 1;
				}

				if (Projectile.timeLeft % 5 == 0)
				{
					float mindist = 0;
					NPC closest = null;
					foreach (var npc in hit)
					{
						if (npc != null)
						{
							if (npc.active && (!npc.townNPC || !npc.friendly))
							{
								float distance = (npc.Center - Projectile.Center).Length();
								if (mostRecent == null)
								{
									if (distance > mindist)
									{
										closest = npc;
										mindist = distance;
									}
								}
								else
								{
									float maxdistance = (mostRecent.Center - Projectile.Center).Length();
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
						mostRecent = closest;
						if (mostRecent.active)
							SpiritMod.primitives.CreateTrail(new AnimePrimTrailTwo(mostRecent));
					}
					else if (Projectile.timeLeft > 15)
						Projectile.timeLeft = 15;
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[Projectile.owner];
			if (charge > 60 && charge < MAXCHARGE)
				return base.Colliding(projHitbox, targetHitbox);
			if (!player.channel)
				return true;
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			Player player = Main.player[Projectile.owner];
			if (player.channel || Projectile.timeLeft > 5)
				return false;
			foreach (var npc in hit)
				if (target == npc)
					return base.CanHitNPC(target);
			return false;
		}

		public override void Kill(int timeLeft) => Main.player[Projectile.owner].GetModPlayer<MyPlayer>().AnimeSword = false;
		public override bool PreDraw(ref Color lightColor) => false;
		public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(direction);
		public override void ReceiveExtraAI(BinaryReader reader) => direction = reader.ReadVector2();
	}

	public class AnimeSwordLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<AnimeSword>())
				BladeoftheDragonLayer.DrawItem(Mod.Assets.Request<Texture2D>("Items/Weapon/Swung/AnimeSword/AnimeSwordProj").Value, Mod.Assets.Request<Texture2D>("Items/Sets/SwordsMisc/BladeOfTheDragon/BladeOfTheDragon_sparkle").Value, drawInfo);
		}
	}
}