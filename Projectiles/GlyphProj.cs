using SpiritMod.Dusts;
using SpiritMod.Items;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class GlyphProj : GlobalProjectile
	{
		private static bool hasNext = false;
		private static int nextType;
		private static GlyphType nextGlyph;

        public GlyphType Glyph { get; private set; }

        public override bool InstancePerEntity => true;

		public override void SetDefaults(Projectile projectile)
		{
			bool send = true;

			if (MyPlayer.swingingCheck && MyPlayer.swingingItem != null)
				Glyph = MyPlayer.swingingItem.GetGlobalItem<GItem>().Glyph;
			else if (Main.ProjectileUpdateLoopIndex >= 0)
			{
				Projectile source = Main.projectile[Main.ProjectileUpdateLoopIndex];
				if (source.active && source.owner == Main.myPlayer)
					Glyph = source.GetGlobalProjectile<GlyphProj>().Glyph;
			}
			else if (hasNext)
			{
				send = false;
				hasNext = false;
				if (projectile.type == nextType)
					Glyph = nextGlyph;
			}
			else
				Glyph = 0;

			if (send && Glyph != 0 && Main.netMode != NetmodeID.SinglePlayer)
			{
				ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.ProjectileData, 3);
				packet.Write((short)projectile.type);
				packet.Write((byte)Glyph);
				packet.Send();
			}
		}

		internal static void ReceiveProjectileData(BinaryReader reader, int sender)
		{
			if (Main.netMode != NetmodeID.Server)
				return;

			hasNext = true;
			nextType = reader.ReadInt16();
			nextGlyph = (GlyphType)reader.ReadByte();

			ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.ProjectileData, 3);
			packet.Write((short)nextType);
			packet.Write((byte)nextGlyph);
			packet.Send(-1, sender);
		}

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

			if (Glyph == GlyphType.Unholy)
				Items.Glyphs.UnholyGlyph.PlagueEffects(target, projectile.owner, ref damage, crit);
			else if (Glyph == GlyphType.Phase)
				Items.Glyphs.PhaseGlyph.PhaseEffects(player, ref damage, crit);
			else if (Glyph == GlyphType.Daze)
				Items.Glyphs.DazeGlyph.Daze(target, ref damage);
			else if (Glyph == GlyphType.Radiant)
				Items.Glyphs.RadiantGlyph.DivineStrike(player, ref damage);

			if (modPlayer.reachSet && target.life <= target.life / 2 && projectile.thrown && crit)
				damage = (int)(damage * 2.25f);
		}

		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

			if (modPlayer.jellynautHelm && modPlayer.jellynautStacks < 4 && projectile.magic && (target.life <= 0 || Main.rand.NextBool(8)) && !target.friendly && !target.SpawnedFromStatue)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int p = Projectile.NewProjectile(player.position.X + Main.rand.Next(-20, 20), player.position.Y + Main.rand.Next(-20, 0), 1, -1, ModContent.ProjectileType<Projectiles.Magic.JellynautOrbiter>(), 0, 0, Main.myPlayer);
					Main.projectile[p].scale = Main.rand.NextFloat(.5f, 1f);
					modPlayer.jellynautStacks++;
				}
			}

			if (modPlayer.AceOfHearts && target.life <= 0 && crit && !target.friendly && target.lifeMax > 15 && !target.SpawnedFromStatue)
			{
				ItemUtils.NewItemWithSync(projectile.owner, (int)target.position.X, (int)target.position.Y, target.width, target.height, Main.halloween ? ItemID.CandyApple : ItemID.Heart);
				for (int i = 0; i < 3; i++)
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<HeartDust>(), 0, -0.8f);
			}

			if (modPlayer.AceOfDiamonds && target.life <= 0 && crit && !target.friendly && target.lifeMax > 15 && !target.SpawnedFromStatue)
			{
				ItemUtils.NewItemWithSync(projectile.owner, (int)target.position.X, (int)target.position.Y, target.width, target.height, ModContent.ItemType<Items.Accessory.AceCardsSet.DiamondAce>());
				for (int i = 0; i < 3; i++)
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<DiamondDust>(), 0, -0.8f);
			}

			switch (Glyph)
			{
				case GlyphType.Frost:
					Items.Glyphs.FrostGlyph.CreateIceSpikes(player, target, crit);
					break;
				case GlyphType.Sanguine:
					Items.Glyphs.SanguineGlyph.BloodCorruption(Main.player[projectile.owner], target, damage);
					break;
				case GlyphType.Blaze:
					Items.Glyphs.BlazeGlyph.Rage(player, target);
					break;
				case GlyphType.Bee:
					if (projectile.type != ProjectileID.Bee && projectile.type != ProjectileID.GiantBee)
						Items.Glyphs.BeeGlyph.ReleaseBees(player, target, damage);
					break;
			}

			if (projectile.friendly && projectile.thrown && Main.rand.NextBool(8) && player.GetSpiritPlayer().geodeSet == true)
				target.AddBuff(24, 150);
			if (projectile.friendly && projectile.thrown && Main.rand.NextBool(8) && player.GetSpiritPlayer().geodeSet == true)
				target.AddBuff(44, 150);
		}

		public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit)
		{
			Player player = Main.player[projectile.owner];
			if (Glyph == GlyphType.Phase)
				Items.Glyphs.PhaseGlyph.PhaseEffects(player, ref damage, crit);
			else if (Glyph == GlyphType.Daze)
				Items.Glyphs.DazeGlyph.Daze(target, ref damage);
			else if (Glyph == GlyphType.Radiant)
				Items.Glyphs.RadiantGlyph.DivineStrike(player, ref damage);
		}

		public override void OnHitPvp(Projectile projectile, Player target, int damage, bool crit)
		{
			Player player = Main.player[projectile.owner];
			switch (Glyph)
			{
				case GlyphType.Sanguine:
					Items.Glyphs.SanguineGlyph.BloodCorruption(Main.player[projectile.owner], target, damage);
					break;
				case GlyphType.Blaze:
					Items.Glyphs.BlazeGlyph.Rage(player);
					break;
			}
		}
	}
}
