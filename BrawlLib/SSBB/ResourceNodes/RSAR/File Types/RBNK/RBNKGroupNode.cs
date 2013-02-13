﻿using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RBNKEntryNode : ResourceNode
    {
        internal VoidPtr _offset;
        internal VoidPtr _rebuildBase;
        internal RBNKNode RBNKNode
        {
            get
            {
                ResourceNode n = this;
                while (((n = n.Parent) != null) && !(n is RBNKNode)) ;
                return n as RBNKNode;
            }
        }
    }

    public unsafe class RBNKDataGroupNode : ResourceNode
    {
        internal RBNK_DATAHeader* Header { get { return (RBNK_DATAHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RBNKGroup; } }

        protected override bool OnInitialize()
        {
            _name = "Data";

            return Header->_list._numEntries > 0;
        }

        protected override void OnPopulate()
        {
            RBNK_DATAHeader* header = Header;
            VoidPtr offset = &header->_list;
            int count = header->_list._numEntries;

            LabelItem[] list = ((RBNKNode)_parent)._labels; //Get labels from parent
            ((RBNKNode)_parent)._labels = null; //Clear labels, no more use for them!

            for (int i = 0; i < count; i++)
            {
                ruint entry = header->_list.Entries[i];
                RBNKEntryNode e = null;
                switch (entry._dataType) //RegionTableType
                {
                    default:
                        e = new RBNKNullNode();
                        break;
                    case 1: //InstParam
                        e = new RBNKDataInstParamNode();
                        if (list != null)
                            (e as RBNKDataInstParamNode)._key = (byte)list[i].Tag;
                        break;
                    case 2: //RangeTable
                        e = new RBNKDataRangeTableNode();
                        break;
                    case 3: //IndexTable
                        e = new RBNKDataIndexTableNode();
                        break;
                }
                if (e != null)
                {
                    e._offset = offset;
                    if (list != null)
                        e._name = list[i].String;
                    e.Initialize(this, header->_list.Get(offset, i), 0);
                }
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 0xC;
            foreach (RBNKEntryNode g in Children)
                size += 8 + g.CalculateSize(true);
            return size.Align(0x20);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RBNK_DATAHeader* header = (RBNK_DATAHeader*)address;

            header->_tag = RBNK_DATAHeader.Tag;
            header->_length = length;
            header->_list._numEntries = Children.Count;

            VoidPtr addr = address + 12 + 8 * Children.Count;
            foreach (RBNKEntryNode g in Children)
            {
                g._rebuildBase = header->_list.Address;
                header->_list.Entries[g.Index] = (int)(addr - header->_list.Address);

                if (g is RBNKDataInstParamNode)
                    header->_list.Entries[g.Index]._dataType = 1;
                else if (g is RBNKDataRangeTableNode)
                    header->_list.Entries[g.Index]._dataType = 2;
                else if (g is RBNKDataIndexTableNode)
                    header->_list.Entries[g.Index]._dataType = 3;
                else if (g is RBNKNullNode)
                    header->_list.Entries[g.Index]._dataType = 4;
                else
                    header->_list.Entries[g.Index]._dataType = 0;

                g.Rebuild(addr, g._calcSize, false);
                addr += g._calcSize;
            }
        }
    }

    public unsafe class RBNKSoundGroupNode : ResourceNode
    {
        internal RBNK_WAVEHeader* Header { get { return (RBNK_WAVEHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RSARFileSoundGroup; } }

        public VoidPtr _audioAddr;

        protected override bool OnInitialize()
        {
            _name = "Audio";

            return Header->_list._numEntries > 0;
        }

        protected override void OnPopulate()
        {
            VoidPtr offset = &Header->_list;
            for (int i = 0; i < Header->_list._numEntries; i++)
                new WAVESoundNode() { _offset = offset }.Initialize(this, Header->_list.Get(offset, i), 0);
            foreach (WAVESoundNode n in Children)
                n.GetAudio();
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 0xC + Children.Count * 8;
            foreach (WAVESoundNode g in Children)
                size += g.WorkingUncompressed.Length;
            return size.Align(0x20);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            uint offset = 0;
            RBNK_WAVEHeader* header = (RBNK_WAVEHeader*)address;
            header->_tag = WAVEHeader.Tag;
            header->_list._numEntries = Children.Count;
            header->_length = (uint)length;
            ruint* table = header->_list.Entries;
            VoidPtr addr = (VoidPtr)(table + Children.Count);
            foreach (WAVESoundNode r in Children)
            {
                //Set offset and write header data
                table[r.Index] = (uint)(addr - header->_list.Address);
                Memory.Move(addr, r.WorkingUncompressed.Address, (uint)r.WorkingUncompressed.Length);

                //Set the offset to the audio samples
                WaveInfo* wave = (WaveInfo*)addr;
                wave->_dataLocation = offset;
                offset += (uint)r._audioSource.Length;

                //Write audio samples
                Memory.Move(_audioAddr, r._audioSource.Address, (uint)r._audioSource.Length);
                _audioAddr += r._audioSource.Length;

                //Advance
                addr += r.WorkingUncompressed.Length;
            }
        }
    }
}
