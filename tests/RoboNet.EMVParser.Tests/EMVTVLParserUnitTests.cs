﻿namespace RoboNET.EMVParser.Tests;

public class EMVTVLParserUnitTests
{
    [Theory]
    [InlineData(
        "5F2A02097882021C00950580800088009A032110149C01009F02060000000020219F03060000000000009F0902008C9F100706010A03A480109F1A0202769F26080123456789ABCDEF9F2701809F3303E0F0C89F34034103029F3501229F3602003E9F37040F00BA209F41030010518407A0000000031010",
        new byte[] { 95, 42 }, 2, DataType.PrimitiveDataObject, ClassType.ApplicationClass)]
    [InlineData(
        "6F478409A00000005945430100A53A50086769726F636172648701019F38069F02069F1D025F2D046465656EBF0C1A9F4D02190A9F6E07028000003030009F0A0800010501000000009000",
        new byte[] { 111 }, 1, DataType.ConstructedDataObject, ClassType.ApplicationClass)]
    [InlineData(
        "82021C00",
        new byte[] { 130 }, 1, DataType.PrimitiveDataObject, ClassType.ContextSpecificClass)]
    [InlineData(
        "9F26080123456789ABCDEF",
        new byte[] { 159, 38 }, 2, DataType.PrimitiveDataObject,
        ClassType.ContextSpecificClass)]
    public void TestParseTag(string sample,
        byte[] expectedData,
        int expectedBytes,
        DataType exptectedDataType,
        ClassType expectedCalssType)
    {
        var data = Convert.FromHexString(sample).AsMemory();

        var result = EMVTLVParser.ParseTagRange(data, out int bytes, out var dataType, out var classType);

        var value = data.Slice(result.Start.Value, result.End.Value - result.Start.Value);
        Assert.Equal(expectedData, value);
        Assert.Equal(expectedBytes, bytes);
        Assert.Equal(exptectedDataType, dataType);
        Assert.Equal(expectedCalssType, classType);
    }

    [Theory]
    [InlineData(
        "5F2A02097882021C00950580800088009A032110149C01009F02060000000020219F03060000000000009F0902008C9F100706010A03A480109F1A0202769F26080123456789ABCDEF9F2701809F3303E0F0C89F34034103029F3501229F3602003E9F37040F00BA209F41030010518407A0000000031010",
        2, 1)]
    [InlineData(
        "6F478409A00000005945430100A53A50086769726F636172648701019F38069F02069F1D025F2D046465656EBF0C1A9F4D02190A9F6E07028000003030009F0A0800010501000000009000",
        71, 1)]
    [InlineData(
        "82021C00",
        2, 1)]
    [InlineData(
        "9F26080123456789ABCDEF",
        8, 1)]
    public void TestParseTagLength(string sample, int expectedlength, int expectedBytes)
    {
        var data = Convert.FromHexString(sample).AsMemory();

        _ = EMVTLVParser.ParseTagRange(data, out int bytes, out _, out _);

        var tagLength = EMVTLVParser.ParseTagLength(data.Slice(bytes), out bytes);
        
        Assert.Equal(expectedlength, tagLength);
        Assert.Equal(expectedBytes, bytes);
    }

    [Theory]
    [InlineData(
        "5F2A02097882021C00950580800088009A032110149C01009F02060000000020219F03060000000000009F0902008C9F100706010A03A480109F1A0202769F26080123456789ABCDEF9F2701809F3303E0F0C89F34034103029F3501229F3602003E9F37040F00BA209F41030010518407A0000000031010",
        "5F2A", "0978")]
    [InlineData(
        "6F478409A00000005945430100A53A50086769726F636172648701019F38069F02069F1D025F2D046465656EBF0C1A9F4D02190A9F6E07028000003030009F0A0800010501000000009000",
        "6F", "8409A00000005945430100A53A50086769726F636172648701019F38069F02069F1D025F2D046465656EBF0C1A9F4D02190A9F6E07028000003030009F0A080001050100000000")]
    [InlineData(
        "9F1A0206439F1B04000000009F3704CFDFCC579F6604240040008407A0000000031010500A566973612044656269748701015F2D047275656E9F1101019F120A566973612044656269749F380C9F66049F02069F37045F2A029F5A0560064306439F0607A00000000310109C01409F02060000000009009F03060000000000009A032502215F2A0206439505000000000082022020940C10010200100505001803040157114555550000003082D281220112594471005F2002202F9F1017060111039000000F4400000000000000000000E6C2102B9F6E04207000009F26085D497322B617CEAC9F2701409F360200B49F4B8180B785A16ADEFDD4561925E6033ED6EB774285A4E40A3D876DDEA917B3A7FF08AB36C70CA2CB51D27FFA69E7FEE3C7BA6638E8EA0B009833D59341A70FA458408C07812C69F579BB5EC1444FFC2565BE67E7D4081BF521283B7352FEECC697DF96AA4710A62065A88F4C0E6A9EC7F6582AAA265098EA97419641D29F24C0DB3A2B9F6C022000908180185AF8F770B39E5131E9A345F50496669A23F0A035FFEA85631BA1A662246F784770E1694E06019A5720A0DF8E75349255A3ACECC2D574BCC93B8495FF4A08821F43BC1579A42BFC33D3F58BF79192C1CB18269E9E1FCDF0780EEE842633F7AB733481FF3E300370CEE3FC8536F5DE100A7D214CFF10294EB30B5FF3204925328F01559F320103926470F564C3B12459A2A76F4B8907674856E42E73FBDF81DCBCF804B47D7DEF604F726F4D9A784E000F5071B9C92A77E678331A921C634D18E85F2358CD0B3110BEAB5803F1C26E6775B1863723C4381D1159998039CF749256F197971FA280BBC1D3334E619F4681C0A5DF22817F0D95D5993D742E0DBD881820204EA889BDDFEFE3ED8F1156DD4D18BF2D5D1A19DFDEE213CB29EB9B11C20297F15144407EB5DADA27F508D9953A6745F3064EB608A37E4F1BF2D07E1AE3F157014A2485375D4441B85E8095D8E9BB53CD362F8C0D3F6D5076BE605CEFD91D0385917DE792EE2AB98CDF47D0794D6354F072398159AB03AD40D3CF984F157BA07E11D95217C30DA1D7456B8CB7DBD03E4FFBF5E598D197406451BF557314D4768748429B6548D1B193A972275DC10F9F4701035A0845555500000030825F24032812315F3401009F4A01825F280206439F0702C2809F690701C912B59F20009F1C0B4246535465726D696E616C9F33036040009F4005AE80905000DF5502A1009F350114DF817A0100DF817B03000000DF81310100DF81320101DF812306000000001000DF812406000000001000DF812606000000001000DF813001FC9F09020001DF811B03000000DF812D03000000DF660400000000DF680100DF670100DF690400000000DF6A06000000000000DF6B06000000000000DF6C060000000000005F3601029F21031828239F34033F00009F390107DF1701089F0D05BCC8FC98009F0E0500100000009F0F05BCC8FC98008E14000000000000000042034103450344005E001F005F25032412019F080200A08C159F02069F03069F1A0295055F2A029A039C019F37048D178A029F02069F03069F1A0295055F2A029A039C019F37045F300202019F440102DF410101",
        "9F4B", "B785A16ADEFDD4561925E6033ED6EB774285A4E40A3D876DDEA917B3A7FF08AB36C70CA2CB51D27FFA69E7FEE3C7BA6638E8EA0B009833D59341A70FA458408C07812C69F579BB5EC1444FFC2565BE67E7D4081BF521283B7352FEECC697DF96AA4710A62065A88F4C0E6A9EC7F6582AAA265098EA97419641D29F24C0DB3A2B")]
    public void TestParseTagList(string sample, string expectedTag, string expectedValue)
    {
        var data = Convert.FromHexString(sample).AsMemory();

        var result = EMVTLVParser.ParseTagsList(data);

        var tag = result.FirstOrDefault(t => string.Equals(t.Tag, expectedTag, StringComparison.Ordinal));
        Assert.NotNull(tag);
        Assert.Equal(expectedTag, Convert.ToHexString(tag.TagData.Span));

        Assert.Equal(expectedValue, Convert.ToHexString(tag.ValueData.Span));
        Assert.Equal(expectedValue, tag.Value);
    }
    
    [Theory]
    [InlineData(
        "5F2A02097882021C00950580800088009A032110149C01009F02060000000020219F03060000000000009F0902008C9F100706010A03A480109F1A0202769F26080123456789ABCDEF9F2701809F3303E0F0C89F34034103029F3501229F3602003E9F37040F00BA209F41030010518407A0000000031010",
        "5F2A", "0978")]
    [InlineData(
        "6F478409A00000005945430100A53A50086769726F636172648701019F38069F02069F1D025F2D046465656EBF0C1A9F4D02190A9F6E07028000003030009F0A0800010501000000009000",
        "6F", "8409A00000005945430100A53A50086769726F636172648701019F38069F02069F1D025F2D046465656EBF0C1A9F4D02190A9F6E07028000003030009F0A080001050100000000")]
    [InlineData(
        "6F478409A00000005945430100A53A50086769726F636172648701019F38069F02069F1D025F2D046465656EBF0C1A9F4D02190A9F6E07028000003030009F0A0800010501000000009000",
        "9F38", "9F02069F1D02")]
    [InlineData(
        "9F1A0206439F1B04000000009F3704CFDFCC579F6604240040008407A0000000031010500A566973612044656269748701015F2D047275656E9F1101019F120A566973612044656269749F380C9F66049F02069F37045F2A029F5A0560064306439F0607A00000000310109C01409F02060000000009009F03060000000000009A032502215F2A0206439505000000000082022020940C10010200100505001803040157114555550000003082D281220112594471005F2002202F9F1017060111039000000F4400000000000000000000E6C2102B9F6E04207000009F26085D497322B617CEAC9F2701409F360200B49F4B8180B785A16ADEFDD4561925E6033ED6EB774285A4E40A3D876DDEA917B3A7FF08AB36C70CA2CB51D27FFA69E7FEE3C7BA6638E8EA0B009833D59341A70FA458408C07812C69F579BB5EC1444FFC2565BE67E7D4081BF521283B7352FEECC697DF96AA4710A62065A88F4C0E6A9EC7F6582AAA265098EA97419641D29F24C0DB3A2B9F6C022000908180185AF8F770B39E5131E9A345F50496669A23F0A035FFEA85631BA1A662246F784770E1694E06019A5720A0DF8E75349255A3ACECC2D574BCC93B8495FF4A08821F43BC1579A42BFC33D3F58BF79192C1CB18269E9E1FCDF0780EEE842633F7AB733481FF3E300370CEE3FC8536F5DE100A7D214CFF10294EB30B5FF3204925328F01559F320103926470F564C3B12459A2A76F4B8907674856E42E73FBDF81DCBCF804B47D7DEF604F726F4D9A784E000F5071B9C92A77E678331A921C634D18E85F2358CD0B3110BEAB5803F1C26E6775B1863723C4381D1159998039CF749256F197971FA280BBC1D3334E619F4681C0A5DF22817F0D95D5993D742E0DBD881820204EA889BDDFEFE3ED8F1156DD4D18BF2D5D1A19DFDEE213CB29EB9B11C20297F15144407EB5DADA27F508D9953A6745F3064EB608A37E4F1BF2D07E1AE3F157014A2485375D4441B85E8095D8E9BB53CD362F8C0D3F6D5076BE605CEFD91D0385917DE792EE2AB98CDF47D0794D6354F072398159AB03AD40D3CF984F157BA07E11D95217C30DA1D7456B8CB7DBD03E4FFBF5E598D197406451BF557314D4768748429B6548D1B193A972275DC10F9F4701035A0845555500000030825F24032812315F3401009F4A01825F280206439F0702C2809F690701C912B59F20009F1C0B4246535465726D696E616C9F33036040009F4005AE80905000DF5502A1009F350114DF817A0100DF817B03000000DF81310100DF81320101DF812306000000001000DF812406000000001000DF812606000000001000DF813001FC9F09020001DF811B03000000DF812D03000000DF660400000000DF680100DF670100DF690400000000DF6A06000000000000DF6B06000000000000DF6C060000000000005F3601029F21031828239F34033F00009F390107DF1701089F0D05BCC8FC98009F0E0500100000009F0F05BCC8FC98008E14000000000000000042034103450344005E001F005F25032412019F080200A08C159F02069F03069F1A0295055F2A029A039C019F37048D178A029F02069F03069F1A0295055F2A029A039C019F37045F300202019F440102DF410101",
        "9F4B", "B785A16ADEFDD4561925E6033ED6EB774285A4E40A3D876DDEA917B3A7FF08AB36C70CA2CB51D27FFA69E7FEE3C7BA6638E8EA0B009833D59341A70FA458408C07812C69F579BB5EC1444FFC2565BE67E7D4081BF521283B7352FEECC697DF96AA4710A62065A88F4C0E6A9EC7F6582AAA265098EA97419641D29F24C0DB3A2B")]
    public void TestReadTagValue(string sample, string expectedTag, string expectedValue)
    {
        var data = Convert.FromHexString(sample).AsMemory();

        var result = EMVTLVParser.ReadTagValue(data, expectedTag);

        Assert.Equal(expectedValue, Convert.ToHexString(result.Span));
    }
}