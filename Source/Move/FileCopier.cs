/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.IO;

public class FileCopier
    {
       static void CopyFile(string sourceFileName, string destFileName, CopyType copyType, string typeFile = "")
        {
            Console.WriteLine($"{typeFile}cp: {copyType} - {sourceFileName} -> {destFileName}");
            switch (copyType)
            {
                case CopyType.Always:
                    File.Copy(sourceFileName, destFileName, true);
                    break;
                case CopyType.IfNewer:
                    if (!File.Exists(destFileName))
                    {
                        File.Copy(sourceFileName, destFileName);
                    }
                    else
                    {
                        var srcInfo = new FileInfo(sourceFileName);
                        var dstInfo = new FileInfo(destFileName);

                        if (srcInfo.LastWriteTime.Ticks > dstInfo.LastWriteTime.Ticks || srcInfo.Length > dstInfo.Length)
                            File.Copy(sourceFileName, destFileName, true);
                        else
                            Console.WriteLine($"    skipping: {sourceFileName}");
                    }
                    break;
                default:
                    File.Copy(sourceFileName, destFileName);
                    break;
            }

        }

    }

}