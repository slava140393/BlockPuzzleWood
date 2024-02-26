using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Utilities
{
	public class FlexibleGridLayout : LayoutGroup
	{
		[Range(0, 1)] public float ColliderProportion;
		public enum FitType
		{
			Uniform,
			Width,
			Height,
			FixedRows,
			FixedColumns,
		}
		public int Rows;
		public int Columns;
		public Vector2 CellSize;
		public Vector2 Spacing;
		public FitType Fit;
		public bool FitX;
		public bool FitY;

		public override void CalculateLayoutInputVertical()
		{
			SetupRowsAndColumns();
			SetupCellSize();
			SetupCellsPosition();

		}
		public override void SetLayoutHorizontal()
		{
		}
		public override void SetLayoutVertical()
		{
		}
		private void SetupCellSize()
		{
			float parentWidth = rectTransform.rect.width;
			float parentHeight = rectTransform.rect.height;

			float cellWidth = (parentWidth / (float)Columns) - ((Spacing.x / (float)Columns) * (Columns - 1)) - (padding.left / (float)Columns) - (padding.right / (float)Columns);
			float cellHeight = (parentHeight / (float)Rows) - ((Spacing.y / (float)Rows) * (Rows - 1)) - (padding.top / (float)Rows) - (padding.bottom / (float)Rows);

			CellSize.x = FitX ? cellWidth : CellSize.x;
			CellSize.y = FitY ? cellHeight : CellSize.y;
		}
		private void SetupRowsAndColumns()
		{
			if(Fit == FitType.Width || Fit == FitType.Height || Fit == FitType.Uniform)
			{
				float sqrRt = Mathf.Sqrt(transform.childCount);
				Rows = Mathf.CeilToInt(sqrRt);
				Columns = Mathf.CeilToInt(sqrRt);

			}

			if(Fit == FitType.Width || Fit == FitType.FixedColumns)
			{
				Rows = Mathf.CeilToInt(transform.childCount / (float)Columns);
			}

			if(Fit == FitType.Height || Fit == FitType.FixedRows)
			{
				Columns = Mathf.CeilToInt(transform.childCount / (float)Rows);
			}
		}
		private void SetupCellsPosition()
		{
			int columnCount = 0;
			int rowCount = 0;

			for( int i = 0; i < rectChildren.Count; i++ )
			{
				rowCount = i / Columns;
				columnCount = i % Columns;

				RectTransform item = rectChildren[i];
				float xPos = (CellSize.x * columnCount) + (Spacing.x * columnCount) + padding.left;
				float yPos = (CellSize.y * rowCount) + (Spacing.y * rowCount) + padding.top;

				SetChildAlongAxis(item, 0, xPos, CellSize.x);
				SetChildAlongAxis(item, 1, yPos, CellSize.y);

				if(item.TryGetComponent(out BoxCollider2D collider))
				{
					collider.size = CellSize * ColliderProportion;
				}
			}
		}
	}
}