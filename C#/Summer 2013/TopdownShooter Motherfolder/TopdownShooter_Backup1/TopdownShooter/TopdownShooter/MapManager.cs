using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.IO;
using System.Xml;

namespace TopdownShooter
{
    class MapManager
    {
        //properties
        ContentManager content;

        //constructor
        public MapManager(ContentManager myContent)
        {
            content = myContent;
        }

        //getters and setters

        //methods
        public string[,] LoadMapFromXML(string filename)
        {
            string[,] map = null;
            int dimX = 0, dimY = 0;
            bool readingRootNode = true;

            using (XmlReader reader = XmlReader.Create("Content\\" + filename))
            {
                while (reader.Read()) //will move from node to node
                {
                    if (readingRootNode)
                    {
                        readingRootNode = false;

                        if (reader.ReadElementString() == "Load")
                        {
                            //be sure the XML file is a map file
                            if (reader.HasAttributes && reader.AttributeCount == 1 && reader.GetAttribute(0) == "Map")
                                continue;
                            else
                                throw new InvalidDataException("Not a map file");
                        }
                        else
                            throw new InvalidDataException("Missing \"Load\" root node");
                    }

                    switch (reader.ReadElementString())
                    {
                        case "x":
                            dimX = reader.ReadContentAsInt();
                            break;
                        case "y":
                            dimY = reader.ReadContentAsInt();
                            break;
                        case "content":
                            if (dimX > 0 && dimY > 0) //should have found x and y by now
                                map = new string[dimX, dimY];
                            else
                                throw new InvalidDataException("Couldn't find x and y dimensions before content");

                            for (int y = 0; y < dimY; y++) //read the data
                            {
                                string[] line = reader.ReadContentAsString().Split(',');

                                if (line.Length != dimX)
                                    throw new InvalidDataException("Map line " + (y + 1).ToString() + " wrong length");

                                for (int x = 0; x < dimX; x++)
                                {
                                    map[x, y] = line[x];
                                }
                            }
                            break;
                    }
                }
            }

            return map;
        }
    }
}
