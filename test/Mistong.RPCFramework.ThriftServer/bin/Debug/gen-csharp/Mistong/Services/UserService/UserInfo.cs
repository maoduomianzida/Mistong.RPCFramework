/**
 * Autogenerated by Thrift Compiler (0.10.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Mistong.Services.UserService
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class UserInfo : TBase
  {

    public int? UserID { get; set; }

    public string UserName { get; set; }

    public bool? Sex { get; set; }

    public UserInfo() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.I32) {
                UserID = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                UserName = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Bool) {
                Sex = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("UserInfo");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (UserID != null) {
          field.Name = "UserID";
          field.Type = TType.I32;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(UserID.Value);
          oprot.WriteFieldEnd();
        }
        if (UserName != null) {
          field.Name = "UserName";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(UserName);
          oprot.WriteFieldEnd();
        }
        if (Sex != null) {
          field.Name = "Sex";
          field.Type = TType.Bool;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(Sex.Value);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("UserInfo(");
      bool __first = true;
      if (UserID != null) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("UserID: ");
        __sb.Append(UserID);
      }
      if (UserName != null) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("UserName: ");
        __sb.Append(UserName);
      }
      if (Sex != null) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Sex: ");
        __sb.Append(Sex);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
