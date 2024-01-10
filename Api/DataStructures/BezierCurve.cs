using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SupernovaMod.Api.DataStructures
{
	public class BezierCurve
	{
		// Token: 0x0600ACF5 RID: 44277 RVA: 0x005D6B35 File Offset: 0x005D4D35
		public BezierCurve(params Vector2[] controls)
		{
			this.ControlPoints = controls;
		}

		// Token: 0x0600ACF6 RID: 44278 RVA: 0x005D6B44 File Offset: 0x005D4D44
		public Vector2 Evaluate(float interpolant)
		{
			return this.PrivateEvaluate(this.ControlPoints, MathHelper.Clamp(interpolant, 0f, 1f));
		}

		// Token: 0x0600ACF7 RID: 44279 RVA: 0x005D6B64 File Offset: 0x005D4D64
		public List<Vector2> GetPoints(int totalPoints)
		{
			float perStep = 1f / (float)totalPoints;
			List<Vector2> points = new List<Vector2>();
			for (float step = 0f; step <= 1f; step += perStep)
			{
				points.Add(this.Evaluate(step));
			}
			return points;
		}

		// Token: 0x0600ACF8 RID: 44280 RVA: 0x005D6BA4 File Offset: 0x005D4DA4
		private Vector2 PrivateEvaluate(Vector2[] points, float T)
		{
			while (points.Length > 2)
			{
				Vector2[] nextPoints = new Vector2[points.Length - 1];
				for (int i = 0; i < points.Length - 1; i++)
				{
					nextPoints[i] = Vector2.Lerp(points[i], points[i + 1], T);
				}
				points = nextPoints;
			}
			if (points.Length <= 1)
			{
				return Vector2.Zero;
			}
			return Vector2.Lerp(points[0], points[1], T);
		}

		// Token: 0x04001932 RID: 6450
		public Vector2[] ControlPoints;
	}
}
