using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsLimc.SmootherLight
{
    internal static class LightGridTool
    {
//        private static readonly HashSet<string> _MESH_TILE_IDS = new HashSet<string> {MeshTileConfig.ID, GasPermeableMembraneConfig.ID};

        public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, LightShape shape)
        {
            if (DoesOcclude(cell))
                return;

            var origin = Grid.CellToXY(cell);
            var lightArea = LightArea.Create(range, Vector2.zero);
            ICellEnumerator enumerator = shape switch
            {
                LightShape.Circle => new RectBorder(origin, range),
                LightShape.Cone => new TrapezoidLayer(origin, range),
                _ => throw new ArgumentOutOfRangeException(nameof(shape), shape, null)
            };

            foreach (var point in enumerator)
            {
                /*
                - 不透光，边界判断
                  - 相交，扩充已有扇区
                  - 不相交，添加新扇区
                - 透光，中心判断
                  - 在扇区内，被遮挡
                  - 在扇区外，有光
                 */
                var deltaPoint = point - origin;
                if (!lightArea.InRange(deltaPoint))
                    // exceed
                    continue;

                var pointCell = Grid.PosToCell(point);
                if (DoesOcclude(pointCell))
                    // 不透光，更新障碍
                    lightArea.AddCellBlock(deltaPoint);
                else if (!lightArea.IsBlocking(deltaPoint))
                    // 透光，不被遮挡
                    visiblePoints.Add(pointCell);
            }
        }

        private static bool DoesOcclude(int cell)
        {
            return Grid.IsValidCell(cell) && !Grid.Transparent[cell] && Grid.Solid[cell];
        }
    }
}