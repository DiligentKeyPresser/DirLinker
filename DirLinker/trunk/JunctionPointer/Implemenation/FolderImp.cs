﻿using System;
using System.Collections.Generic;
using System.Linq;
using JunctionPointer.Interfaces;
using System.IO;
using System.Runtime.InteropServices;
using JunctionPointer.Helpers.ClassFactory;

namespace JunctionPointer.Implemenation
{
    class FolderImp : IFolder
    {
        public String FolderPath { get; set; }

        [DllImport("kernel32.dll", SetLastError=true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I1)]
        private static extern bool CreateSymbolicLink(String lpSymlinkFileName, String lpTargetFileName, SYMBOLIC_LINK_FLAG dwFlags);

        public bool CreateLinkToFolderAt(String linkToBeCreated)
        {
            return CreateSymbolicLink(linkToBeCreated, FolderPath, SYMBOLIC_LINK_FLAG.Directory);
        }

        public Boolean FolderExists()
        {
            return Directory.Exists(FolderPath);
        }

        public void CreateFolder()
        {
            Directory.CreateDirectory(FolderPath);
        }

        /// <summary>
        /// Deletes a folder and its children if it exists
        /// </summary>
        /// <param name="foldername"></param>
        public void DeleteFolder()
        {
            Directory.Delete(FolderPath);
        }

        /// <summary>
        /// Gets a list of files in a folder
        /// </summary>
        /// <param name="folderLocation"></param>
        /// <returns></returns>
        public List<IFile> GetFileList()
        {
            List<IFile>  fileList = new List<IFile>();

            foreach (String file in Directory.GetFiles(FolderPath))
            {
                IFile aFile = JunctionPointer.Helpers.ClassFactory.ClassFactory.CreateInstance<IFile>();
                aFile.SetFile(file);
                fileList.Add(aFile);
            }

            return fileList;
             
        }

        /// <summary>
        /// Gets the size of a folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns>The size of a folder</returns>
        public Int64 DirectorySize()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(FolderPath);

            Int64 dirSize = dirInfo.GetFiles().Sum(file => file.Length);
            dirSize += GetSubFolderList().Sum(dir => dir.DirectorySize());

            return dirSize;
        }

        public Int64 FreeSpaceOnDrive(String drive)
        {
            DriveInfo driveInfo = new DriveInfo(drive);
            return driveInfo.AvailableFreeSpace;

        }

        public List<IFolder> GetSubFolderList()
        {
            List<IFolder> folderList = new List<IFolder>();

            foreach (string subFolder in Directory.GetDirectories(FolderPath))
            {
                IFolder folder = ClassFactory.CreateInstance<IFolder>();
                folder.FolderPath = subFolder;
                folderList.Add(folder);
            }

            return folderList;
        }

        public Char[] GetIllegalPathChars()
        {
            return Path.GetInvalidPathChars();
        }

        public Int32 MaxPath()
        {
            return 248; //According to MSDN 248 is the max folder path for Windows, strange there is not a Path member to get this.
        }

        public void SetAttributes(FileAttributes fileAttributes)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(FolderPath);
            dirInfo.Attributes = fileAttributes;
        }
        public FileAttributes GetAttributes()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(FolderPath);
            return dirInfo.Attributes;
        }
        

    }
}