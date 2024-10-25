// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using JetBrains.Annotations;

namespace Extensions.Source.Tools.TexturePacker;

[PublicAPI]
public class GridPacker //: IPacker
{
//	private readonly Settings settings;
//
//	public GridPacker (Settings settings) {
//		this.settings = settings;
//	}
//
//	public List<Page> pack (Array<Rect> inputRects) {
//		return pack(null, inputRects);
//	}
//
//	public Array<Page> pack (ProgressListener progress, Array<Rect> inputRects) {
//		if (!settings.silent) System.out.print("Packing");
//
//		// Rects are packed with right and top padding, so the max size is increased to match. After packing the padding is
//		// subtracted from the page size.
//		int paddingX = settings.paddingX, paddingY = settings.paddingY;
//		int adjustX = paddingX, adjustY = paddingY;
//		if (settings.edgePadding) {
//			if (settings.duplicatePadding) {
//				adjustX -= paddingX;
//				adjustY -= paddingY;
//			} else {
//				adjustX -= paddingX * 2;
//				adjustY -= paddingY * 2;
//			}
//		}
//		int maxWidth = settings.maxWidth + adjustX, maxHeight = settings.maxHeight + adjustY;
//
//		int n = inputRects.size;
//		int cellWidth = 0, cellHeight = 0;
//		for (int i = 0; i < n; i++) {
//			Rect rect = inputRects.get(i);
//			cellWidth = Math.max(cellWidth, rect.width);
//			cellHeight = Math.max(cellHeight, rect.height);
//		}
//		cellWidth += paddingX;
//		cellHeight += paddingY;
//
//		inputRects.reverse();
//
//		Array<Page> pages = new Array();
//		while (inputRects.size > 0) {
//			progress.count = n - inputRects.size + 1;
//			if (progress.update(progress.count, n)) break;
//
//			Page page = packPage(inputRects, cellWidth, cellHeight, maxWidth, maxHeight);
//			page.width -= paddingX;
//			page.height -= paddingY;
//			pages.add(page);
//		}
//		return pages;
//	}
//
//	private Page packPage (Array<Rect> inputRects, int cellWidth, int cellHeight, int maxWidth, int maxHeight) {
//		Page page = new Page();
//		page.outputRects = new Array();
//
//		int n = inputRects.size;
//		int x = 0, y = 0;
//		for (int i = n - 1; i >= 0; i--) {
//			if (x + cellWidth > maxWidth) {
//				y += cellHeight;
//				if (y > maxHeight - cellHeight) break;
//				x = 0;
//			}
//			Rect rect = inputRects.removeIndex(i);
//			rect.x = x;
//			rect.y = y;
//			rect.width += settings.paddingX;
//			rect.height += settings.paddingY;
//			page.outputRects.add(rect);
//			x += cellWidth;
//			page.width = Math.max(page.width, x);
//			page.height = Math.max(page.height, y + cellHeight);
//		}
//
//		// Flip so rows start at top.
//		for (int i = page.outputRects.size - 1; i >= 0; i--) {
//			Rect rect = page.outputRects.get(i);
//			rect.y = page.height - rect.y - rect.height;
//		}
//		return page;
//	}
}

