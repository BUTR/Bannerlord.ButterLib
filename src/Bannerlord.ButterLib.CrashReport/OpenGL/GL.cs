#nullable disable
using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Bannerlord.ButterLib.CrashReportWindow.OpenGL;

internal static unsafe class GL
{
    public static void LoadEntryPoints()
    {
        foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static))
        {
            var name = field.Name.Remove(0, 1);
            var ptr = GLFW.glfwGetProcAddress(name);
            if (ptr != IntPtr.Zero)
                field.SetValue(null, Marshal.GetDelegateForFunctionPointer(ptr, field.FieldType));
        }
    }

    public const int GL_DEPTH_BUFFER_BIT = 0x00000100;
    public const int GL_STENCIL_BUFFER_BIT = 0x00000400;
    public const int GL_COLOR_BUFFER_BIT = 0x00004000;
    public const int GL_FALSE = 0;
    public const int GL_TRUE = 1;
    public const int GL_POINTS = 0x0000;
    public const int GL_LINES = 0x0001;
    public const int GL_LINE_LOOP = 0x0002;
    public const int GL_LINE_STRIP = 0x0003;
    public const int GL_TRIANGLES = 0x0004;
    public const int GL_TRIANGLE_STRIP = 0x0005;
    public const int GL_TRIANGLE_FAN = 0x0006;
    public const int GL_NEVER = 0x0200;
    public const int GL_LESS = 0x0201;
    public const int GL_EQUAL = 0x0202;
    public const int GL_LEQUAL = 0x0203;
    public const int GL_GREATER = 0x0204;
    public const int GL_NOTEQUAL = 0x0205;
    public const int GL_GEQUAL = 0x0206;
    public const int GL_ALWAYS = 0x0207;
    public const int GL_ZERO = 0;
    public const int GL_ONE = 1;
    public const int GL_SRC_COLOR = 0x0300;
    public const int GL_ONE_MINUS_SRC_COLOR = 0x0301;
    public const int GL_SRC_ALPHA = 0x0302;
    public const int GL_ONE_MINUS_SRC_ALPHA = 0x0303;
    public const int GL_DST_ALPHA = 0x0304;
    public const int GL_ONE_MINUS_DST_ALPHA = 0x0305;
    public const int GL_DST_COLOR = 0x0306;
    public const int GL_ONE_MINUS_DST_COLOR = 0x0307;
    public const int GL_SRC_ALPHA_SATURATE = 0x0308;
    public const int GL_NONE = 0;
    public const int GL_FRONT_LEFT = 0x0400;
    public const int GL_FRONT_RIGHT = 0x0401;
    public const int GL_BACK_LEFT = 0x0402;
    public const int GL_BACK_RIGHT = 0x0403;
    public const int GL_FRONT = 0x0404;
    public const int GL_BACK = 0x0405;
    public const int GL_LEFT = 0x0406;
    public const int GL_RIGHT = 0x0407;
    public const int GL_FRONT_AND_BACK = 0x0408;
    public const int GL_NO_ERROR = 0;
    public const int GL_INVALID_ENUM = 0x0500;
    public const int GL_INVALID_VALUE = 0x0501;
    public const int GL_INVALID_OPERATION = 0x0502;
    public const int GL_OUT_OF_MEMORY = 0x0505;
    public const int GL_CW = 0x0900;
    public const int GL_CCW = 0x0901;
    public const int GL_POINT_SIZE = 0x0B11;
    public const int GL_POINT_SIZE_RANGE = 0x0B12;
    public const int GL_POINT_SIZE_GRANULARITY = 0x0B13;
    public const int GL_LINE_SMOOTH = 0x0B20;
    public const int GL_LINE_WIDTH = 0x0B21;
    public const int GL_LINE_WIDTH_RANGE = 0x0B22;
    public const int GL_LINE_WIDTH_GRANULARITY = 0x0B23;
    public const int GL_POLYGON_MODE = 0x0B40;
    public const int GL_POLYGON_SMOOTH = 0x0B41;
    public const int GL_CULL_FACE = 0x0B44;
    public const int GL_CULL_FACE_MODE = 0x0B45;
    public const int GL_FRONT_FACE = 0x0B46;
    public const int GL_DEPTH_RANGE = 0x0B70;
    public const int GL_DEPTH_TEST = 0x0B71;
    public const int GL_DEPTH_WRITEMASK = 0x0B72;
    public const int GL_DEPTH_CLEAR_VALUE = 0x0B73;
    public const int GL_DEPTH_FUNC = 0x0B74;
    public const int GL_STENCIL_TEST = 0x0B90;
    public const int GL_STENCIL_CLEAR_VALUE = 0x0B91;
    public const int GL_STENCIL_FUNC = 0x0B92;
    public const int GL_STENCIL_VALUE_MASK = 0x0B93;
    public const int GL_STENCIL_FAIL = 0x0B94;
    public const int GL_STENCIL_PASS_DEPTH_FAIL = 0x0B95;
    public const int GL_STENCIL_PASS_DEPTH_PASS = 0x0B96;
    public const int GL_STENCIL_REF = 0x0B97;
    public const int GL_STENCIL_WRITEMASK = 0x0B98;
    public const int GL_VIEWPORT = 0x0BA2;
    public const int GL_DITHER = 0x0BD0;
    public const int GL_BLEND_DST = 0x0BE0;
    public const int GL_BLEND_SRC = 0x0BE1;
    public const int GL_BLEND = 0x0BE2;
    public const int GL_LOGIC_OP_MODE = 0x0BF0;
    public const int GL_DRAW_BUFFER = 0x0C01;
    public const int GL_READ_BUFFER = 0x0C02;
    public const int GL_SCISSOR_BOX = 0x0C10;
    public const int GL_SCISSOR_TEST = 0x0C11;
    public const int GL_COLOR_CLEAR_VALUE = 0x0C22;
    public const int GL_COLOR_WRITEMASK = 0x0C23;
    public const int GL_DOUBLEBUFFER = 0x0C32;
    public const int GL_STEREO = 0x0C33;
    public const int GL_LINE_SMOOTH_HINT = 0x0C52;
    public const int GL_POLYGON_SMOOTH_HINT = 0x0C53;
    public const int GL_UNPACK_SWAP_BYTES = 0x0CF0;
    public const int GL_UNPACK_LSB_FIRST = 0x0CF1;
    public const int GL_UNPACK_ROW_LENGTH = 0x0CF2;
    public const int GL_UNPACK_SKIP_ROWS = 0x0CF3;
    public const int GL_UNPACK_SKIP_PIXELS = 0x0CF4;
    public const int GL_UNPACK_ALIGNMENT = 0x0CF5;
    public const int GL_PACK_SWAP_BYTES = 0x0D00;
    public const int GL_PACK_LSB_FIRST = 0x0D01;
    public const int GL_PACK_ROW_LENGTH = 0x0D02;
    public const int GL_PACK_SKIP_ROWS = 0x0D03;
    public const int GL_PACK_SKIP_PIXELS = 0x0D04;
    public const int GL_PACK_ALIGNMENT = 0x0D05;
    public const int GL_MAX_TEXTURE_SIZE = 0x0D33;
    public const int GL_MAX_VIEWPORT_DIMS = 0x0D3A;
    public const int GL_SUBPIXEL_BITS = 0x0D50;
    public const int GL_TEXTURE_1D = 0x0DE0;
    public const int GL_TEXTURE_2D = 0x0DE1;
    public const int GL_TEXTURE_WIDTH = 0x1000;
    public const int GL_TEXTURE_HEIGHT = 0x1001;
    public const int GL_TEXTURE_BORDER_COLOR = 0x1004;
    public const int GL_DONT_CARE = 0x1100;
    public const int GL_FASTEST = 0x1101;
    public const int GL_NICEST = 0x1102;
    public const int GL_BYTE = 0x1400;
    public const int GL_UNSIGNED_BYTE = 0x1401;
    public const int GL_SHORT = 0x1402;
    public const int GL_UNSIGNED_SHORT = 0x1403;
    public const int GL_INT = 0x1404;
    public const int GL_UNSIGNED_INT = 0x1405;
    public const int GL_FLOAT = 0x1406;
    public const int GL_CLEAR = 0x1500;
    public const int GL_AND = 0x1501;
    public const int GL_AND_REVERSE = 0x1502;
    public const int GL_COPY = 0x1503;
    public const int GL_AND_INVERTED = 0x1504;
    public const int GL_NOOP = 0x1505;
    public const int GL_XOR = 0x1506;
    public const int GL_OR = 0x1507;
    public const int GL_NOR = 0x1508;
    public const int GL_EQUIV = 0x1509;
    public const int GL_INVERT = 0x150A;
    public const int GL_OR_REVERSE = 0x150B;
    public const int GL_COPY_INVERTED = 0x150C;
    public const int GL_OR_INVERTED = 0x150D;
    public const int GL_NAND = 0x150E;
    public const int GL_SET = 0x150F;
    public const int GL_TEXTURE = 0x1702;
    public const int GL_COLOR = 0x1800;
    public const int GL_DEPTH = 0x1801;
    public const int GL_STENCIL = 0x1802;
    public const int GL_STENCIL_INDEX = 0x1901;
    public const int GL_DEPTH_COMPONENT = 0x1902;
    public const int GL_RED = 0x1903;
    public const int GL_GREEN = 0x1904;
    public const int GL_BLUE = 0x1905;
    public const int GL_ALPHA = 0x1906;
    public const int GL_RGB = 0x1907;
    public const int GL_RGBA = 0x1908;
    public const int GL_POINT = 0x1B00;
    public const int GL_LINE = 0x1B01;
    public const int GL_FILL = 0x1B02;
    public const int GL_KEEP = 0x1E00;
    public const int GL_REPLACE = 0x1E01;
    public const int GL_INCR = 0x1E02;
    public const int GL_DECR = 0x1E03;
    public const int GL_VENDOR = 0x1F00;
    public const int GL_RENDERER = 0x1F01;
    public const int GL_VERSION = 0x1F02;
    public const int GL_EXTENSIONS = 0x1F03;
    public const int GL_NEAREST = 0x2600;
    public const int GL_LINEAR = 0x2601;
    public const int GL_NEAREST_MIPMAP_NEAREST = 0x2700;
    public const int GL_LINEAR_MIPMAP_NEAREST = 0x2701;
    public const int GL_NEAREST_MIPMAP_LINEAR = 0x2702;
    public const int GL_LINEAR_MIPMAP_LINEAR = 0x2703;
    public const int GL_TEXTURE_MAG_FILTER = 0x2800;
    public const int GL_TEXTURE_MIN_FILTER = 0x2801;
    public const int GL_TEXTURE_WRAP_S = 0x2802;
    public const int GL_TEXTURE_WRAP_T = 0x2803;
    public const int GL_REPEAT = 0x2901;
    public const int GL_COLOR_LOGIC_OP = 0x0BF2;
    public const int GL_POLYGON_OFFSET_UNITS = 0x2A00;
    public const int GL_POLYGON_OFFSET_POINT = 0x2A01;
    public const int GL_POLYGON_OFFSET_LINE = 0x2A02;
    public const int GL_POLYGON_OFFSET_FILL = 0x8037;
    public const int GL_POLYGON_OFFSET_FACTOR = 0x8038;
    public const int GL_TEXTURE_BINDING_1D = 0x8068;
    public const int GL_TEXTURE_BINDING_2D = 0x8069;
    public const int GL_TEXTURE_INTERNAL_FORMAT = 0x1003;
    public const int GL_TEXTURE_RED_SIZE = 0x805C;
    public const int GL_TEXTURE_GREEN_SIZE = 0x805D;
    public const int GL_TEXTURE_BLUE_SIZE = 0x805E;
    public const int GL_TEXTURE_ALPHA_SIZE = 0x805F;
    public const int GL_DOUBLE = 0x140A;
    public const int GL_PROXY_TEXTURE_1D = 0x8063;
    public const int GL_PROXY_TEXTURE_2D = 0x8064;
    public const int GL_R3_G3_B2 = 0x2A10;
    public const int GL_RGB4 = 0x804F;
    public const int GL_RGB5 = 0x8050;
    public const int GL_RGB8 = 0x8051;
    public const int GL_RGB10 = 0x8052;
    public const int GL_RGB12 = 0x8053;
    public const int GL_RGB16 = 0x8054;
    public const int GL_RGBA2 = 0x8055;
    public const int GL_RGBA4 = 0x8056;
    public const int GL_RGB5_A1 = 0x8057;
    public const int GL_RGBA8 = 0x8058;
    public const int GL_RGB10_A2 = 0x8059;
    public const int GL_RGBA12 = 0x805A;
    public const int GL_RGBA16 = 0x805B;
    public const int GL_UNSIGNED_BYTE_3_3_2 = 0x8032;
    public const int GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
    public const int GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;
    public const int GL_UNSIGNED_INT_8_8_8_8 = 0x8035;
    public const int GL_UNSIGNED_INT_10_10_10_2 = 0x8036;
    public const int GL_TEXTURE_BINDING_3D = 0x806A;
    public const int GL_PACK_SKIP_IMAGES = 0x806B;
    public const int GL_PACK_IMAGE_HEIGHT = 0x806C;
    public const int GL_UNPACK_SKIP_IMAGES = 0x806D;
    public const int GL_UNPACK_IMAGE_HEIGHT = 0x806E;
    public const int GL_TEXTURE_3D = 0x806F;
    public const int GL_PROXY_TEXTURE_3D = 0x8070;
    public const int GL_TEXTURE_DEPTH = 0x8071;
    public const int GL_TEXTURE_WRAP_R = 0x8072;
    public const int GL_MAX_3D_TEXTURE_SIZE = 0x8073;
    public const int GL_UNSIGNED_BYTE_2_3_3_REV = 0x8362;
    public const int GL_UNSIGNED_SHORT_5_6_5 = 0x8363;
    public const int GL_UNSIGNED_SHORT_5_6_5_REV = 0x8364;
    public const int GL_UNSIGNED_SHORT_4_4_4_4_REV = 0x8365;
    public const int GL_UNSIGNED_SHORT_1_5_5_5_REV = 0x8366;
    public const int GL_UNSIGNED_INT_8_8_8_8_REV = 0x8367;
    public const int GL_UNSIGNED_INT_2_10_10_10_REV = 0x8368;
    public const int GL_BGR = 0x80E0;
    public const int GL_BGRA = 0x80E1;
    public const int GL_MAX_ELEMENTS_VERTICES = 0x80E8;
    public const int GL_MAX_ELEMENTS_INDICES = 0x80E9;
    public const int GL_CLAMP_TO_EDGE = 0x812F;
    public const int GL_TEXTURE_MIN_LOD = 0x813A;
    public const int GL_TEXTURE_MAX_LOD = 0x813B;
    public const int GL_TEXTURE_BASE_LEVEL = 0x813C;
    public const int GL_TEXTURE_MAX_LEVEL = 0x813D;
    public const int GL_SMOOTH_POINT_SIZE_RANGE = 0x0B12;
    public const int GL_SMOOTH_POINT_SIZE_GRANULARITY = 0x0B13;
    public const int GL_SMOOTH_LINE_WIDTH_RANGE = 0x0B22;
    public const int GL_SMOOTH_LINE_WIDTH_GRANULARITY = 0x0B23;
    public const int GL_ALIASED_LINE_WIDTH_RANGE = 0x846E;
    public const int GL_TEXTURE0 = 0x84C0;
    public const int GL_TEXTURE1 = 0x84C1;
    public const int GL_TEXTURE2 = 0x84C2;
    public const int GL_TEXTURE3 = 0x84C3;
    public const int GL_TEXTURE4 = 0x84C4;
    public const int GL_TEXTURE5 = 0x84C5;
    public const int GL_TEXTURE6 = 0x84C6;
    public const int GL_TEXTURE7 = 0x84C7;
    public const int GL_TEXTURE8 = 0x84C8;
    public const int GL_TEXTURE9 = 0x84C9;
    public const int GL_TEXTURE10 = 0x84CA;
    public const int GL_TEXTURE11 = 0x84CB;
    public const int GL_TEXTURE12 = 0x84CC;
    public const int GL_TEXTURE13 = 0x84CD;
    public const int GL_TEXTURE14 = 0x84CE;
    public const int GL_TEXTURE15 = 0x84CF;
    public const int GL_TEXTURE16 = 0x84D0;
    public const int GL_TEXTURE17 = 0x84D1;
    public const int GL_TEXTURE18 = 0x84D2;
    public const int GL_TEXTURE19 = 0x84D3;
    public const int GL_TEXTURE20 = 0x84D4;
    public const int GL_TEXTURE21 = 0x84D5;
    public const int GL_TEXTURE22 = 0x84D6;
    public const int GL_TEXTURE23 = 0x84D7;
    public const int GL_TEXTURE24 = 0x84D8;
    public const int GL_TEXTURE25 = 0x84D9;
    public const int GL_TEXTURE26 = 0x84DA;
    public const int GL_TEXTURE27 = 0x84DB;
    public const int GL_TEXTURE28 = 0x84DC;
    public const int GL_TEXTURE29 = 0x84DD;
    public const int GL_TEXTURE30 = 0x84DE;
    public const int GL_TEXTURE31 = 0x84DF;
    public const int GL_ACTIVE_TEXTURE = 0x84E0;
    public const int GL_MULTISAMPLE = 0x809D;
    public const int GL_SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
    public const int GL_SAMPLE_ALPHA_TO_ONE = 0x809F;
    public const int GL_SAMPLE_COVERAGE = 0x80A0;
    public const int GL_SAMPLE_BUFFERS = 0x80A8;
    public const int GL_SAMPLES = 0x80A9;
    public const int GL_SAMPLE_COVERAGE_VALUE = 0x80AA;
    public const int GL_SAMPLE_COVERAGE_INVERT = 0x80AB;
    public const int GL_TEXTURE_CUBE_MAP = 0x8513;
    public const int GL_TEXTURE_BINDING_CUBE_MAP = 0x8514;
    public const int GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
    public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
    public const int GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
    public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
    public const int GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
    public const int GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
    public const int GL_PROXY_TEXTURE_CUBE_MAP = 0x851B;
    public const int GL_MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
    public const int GL_COMPRESSED_RGB = 0x84ED;
    public const int GL_COMPRESSED_RGBA = 0x84EE;
    public const int GL_TEXTURE_COMPRESSION_HINT = 0x84EF;
    public const int GL_TEXTURE_COMPRESSED_IMAGE_SIZE = 0x86A0;
    public const int GL_TEXTURE_COMPRESSED = 0x86A1;
    public const int GL_NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
    public const int GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3;
    public const int GL_CLAMP_TO_BORDER = 0x812D;
    public const int GL_BLEND_DST_RGB = 0x80C8;
    public const int GL_BLEND_SRC_RGB = 0x80C9;
    public const int GL_BLEND_DST_ALPHA = 0x80CA;
    public const int GL_BLEND_SRC_ALPHA = 0x80CB;
    public const int GL_POINT_FADE_THRESHOLD_SIZE = 0x8128;
    public const int GL_DEPTH_COMPONENT16 = 0x81A5;
    public const int GL_DEPTH_COMPONENT24 = 0x81A6;
    public const int GL_DEPTH_COMPONENT32 = 0x81A7;
    public const int GL_MIRRORED_REPEAT = 0x8370;
    public const int GL_MAX_TEXTURE_LOD_BIAS = 0x84FD;
    public const int GL_TEXTURE_LOD_BIAS = 0x8501;
    public const int GL_INCR_WRAP = 0x8507;
    public const int GL_DECR_WRAP = 0x8508;
    public const int GL_TEXTURE_DEPTH_SIZE = 0x884A;
    public const int GL_TEXTURE_COMPARE_MODE = 0x884C;
    public const int GL_TEXTURE_COMPARE_FUNC = 0x884D;
    public const int GL_BLEND_COLOR = 0x8005;
    public const int GL_BLEND_EQUATION = 0x8009;
    public const int GL_CONSTANT_COLOR = 0x8001;
    public const int GL_ONE_MINUS_CONSTANT_COLOR = 0x8002;
    public const int GL_CONSTANT_ALPHA = 0x8003;
    public const int GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004;
    public const int GL_FUNC_ADD = 0x8006;
    public const int GL_FUNC_REVERSE_SUBTRACT = 0x800B;
    public const int GL_FUNC_SUBTRACT = 0x800A;
    public const int GL_MIN = 0x8007;
    public const int GL_MAX = 0x8008;
    public const int GL_BUFFER_SIZE = 0x8764;
    public const int GL_BUFFER_USAGE = 0x8765;
    public const int GL_QUERY_COUNTER_BITS = 0x8864;
    public const int GL_CURRENT_QUERY = 0x8865;
    public const int GL_QUERY_RESULT = 0x8866;
    public const int GL_QUERY_RESULT_AVAILABLE = 0x8867;
    public const int GL_ARRAY_BUFFER = 0x8892;
    public const int GL_ELEMENT_ARRAY_BUFFER = 0x8893;
    public const int GL_ARRAY_BUFFER_BINDING = 0x8894;
    public const int GL_ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
    public const int GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
    public const int GL_READ_ONLY = 0x88B8;
    public const int GL_WRITE_ONLY = 0x88B9;
    public const int GL_READ_WRITE = 0x88BA;
    public const int GL_BUFFER_ACCESS = 0x88BB;
    public const int GL_BUFFER_MAPPED = 0x88BC;
    public const int GL_BUFFER_MAP_POINTER = 0x88BD;
    public const int GL_STREAM_DRAW = 0x88E0;
    public const int GL_STREAM_READ = 0x88E1;
    public const int GL_STREAM_COPY = 0x88E2;
    public const int GL_STATIC_DRAW = 0x88E4;
    public const int GL_STATIC_READ = 0x88E5;
    public const int GL_STATIC_COPY = 0x88E6;
    public const int GL_DYNAMIC_DRAW = 0x88E8;
    public const int GL_DYNAMIC_READ = 0x88E9;
    public const int GL_DYNAMIC_COPY = 0x88EA;
    public const int GL_SAMPLES_PASSED = 0x8914;
    public const int GL_SRC1_ALPHA = 0x8589;
    public const int GL_BLEND_EQUATION_RGB = 0x8009;
    public const int GL_VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
    public const int GL_VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
    public const int GL_VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
    public const int GL_VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
    public const int GL_CURRENT_VERTEX_ATTRIB = 0x8626;
    public const int GL_VERTEX_PROGRAM_POINT_SIZE = 0x8642;
    public const int GL_VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
    public const int GL_STENCIL_BACK_FUNC = 0x8800;
    public const int GL_STENCIL_BACK_FAIL = 0x8801;
    public const int GL_STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
    public const int GL_STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
    public const int GL_MAX_DRAW_BUFFERS = 0x8824;
    public const int GL_DRAW_BUFFER0 = 0x8825;
    public const int GL_DRAW_BUFFER1 = 0x8826;
    public const int GL_DRAW_BUFFER2 = 0x8827;
    public const int GL_DRAW_BUFFER3 = 0x8828;
    public const int GL_DRAW_BUFFER4 = 0x8829;
    public const int GL_DRAW_BUFFER5 = 0x882A;
    public const int GL_DRAW_BUFFER6 = 0x882B;
    public const int GL_DRAW_BUFFER7 = 0x882C;
    public const int GL_DRAW_BUFFER8 = 0x882D;
    public const int GL_DRAW_BUFFER9 = 0x882E;
    public const int GL_DRAW_BUFFER10 = 0x882F;
    public const int GL_DRAW_BUFFER11 = 0x8830;
    public const int GL_DRAW_BUFFER12 = 0x8831;
    public const int GL_DRAW_BUFFER13 = 0x8832;
    public const int GL_DRAW_BUFFER14 = 0x8833;
    public const int GL_DRAW_BUFFER15 = 0x8834;
    public const int GL_BLEND_EQUATION_ALPHA = 0x883D;
    public const int GL_MAX_VERTEX_ATTRIBS = 0x8869;
    public const int GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
    public const int GL_MAX_TEXTURE_IMAGE_UNITS = 0x8872;
    public const int GL_FRAGMENT_SHADER = 0x8B30;
    public const int GL_VERTEX_SHADER = 0x8B31;
    public const int GL_MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x8B49;
    public const int GL_MAX_VERTEX_UNIFORM_COMPONENTS = 0x8B4A;
    public const int GL_MAX_VARYING_FLOATS = 0x8B4B;
    public const int GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
    public const int GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
    public const int GL_SHADER_TYPE = 0x8B4F;
    public const int GL_FLOAT_VEC2 = 0x8B50;
    public const int GL_FLOAT_VEC3 = 0x8B51;
    public const int GL_FLOAT_VEC4 = 0x8B52;
    public const int GL_INT_VEC2 = 0x8B53;
    public const int GL_INT_VEC3 = 0x8B54;
    public const int GL_INT_VEC4 = 0x8B55;
    public const int GL_BOOL = 0x8B56;
    public const int GL_BOOL_VEC2 = 0x8B57;
    public const int GL_BOOL_VEC3 = 0x8B58;
    public const int GL_BOOL_VEC4 = 0x8B59;
    public const int GL_FLOAT_MAT2 = 0x8B5A;
    public const int GL_FLOAT_MAT3 = 0x8B5B;
    public const int GL_FLOAT_MAT4 = 0x8B5C;
    public const int GL_SAMPLER_1D = 0x8B5D;
    public const int GL_SAMPLER_2D = 0x8B5E;
    public const int GL_SAMPLER_3D = 0x8B5F;
    public const int GL_SAMPLER_CUBE = 0x8B60;
    public const int GL_SAMPLER_1D_SHADOW = 0x8B61;
    public const int GL_SAMPLER_2D_SHADOW = 0x8B62;
    public const int GL_DELETE_STATUS = 0x8B80;
    public const int GL_COMPILE_STATUS = 0x8B81;
    public const int GL_LINK_STATUS = 0x8B82;
    public const int GL_VALIDATE_STATUS = 0x8B83;
    public const int GL_INFO_LOG_LENGTH = 0x8B84;
    public const int GL_ATTACHED_SHADERS = 0x8B85;
    public const int GL_ACTIVE_UNIFORMS = 0x8B86;
    public const int GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
    public const int GL_SHADER_SOURCE_LENGTH = 0x8B88;
    public const int GL_ACTIVE_ATTRIBUTES = 0x8B89;
    public const int GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
    public const int GL_FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B;
    public const int GL_SHADING_LANGUAGE_VERSION = 0x8B8C;
    public const int GL_CURRENT_PROGRAM = 0x8B8D;
    public const int GL_POINT_SPRITE_COORD_ORIGIN = 0x8CA0;
    public const int GL_LOWER_LEFT = 0x8CA1;
    public const int GL_UPPER_LEFT = 0x8CA2;
    public const int GL_STENCIL_BACK_REF = 0x8CA3;
    public const int GL_STENCIL_BACK_VALUE_MASK = 0x8CA4;
    public const int GL_STENCIL_BACK_WRITEMASK = 0x8CA5;
    public const int GL_PIXEL_PACK_BUFFER = 0x88EB;
    public const int GL_PIXEL_UNPACK_BUFFER = 0x88EC;
    public const int GL_PIXEL_PACK_BUFFER_BINDING = 0x88ED;
    public const int GL_PIXEL_UNPACK_BUFFER_BINDING = 0x88EF;
    public const int GL_FLOAT_MAT2x3 = 0x8B65;
    public const int GL_FLOAT_MAT2x4 = 0x8B66;
    public const int GL_FLOAT_MAT3x2 = 0x8B67;
    public const int GL_FLOAT_MAT3x4 = 0x8B68;
    public const int GL_FLOAT_MAT4x2 = 0x8B69;
    public const int GL_FLOAT_MAT4x3 = 0x8B6A;
    public const int GL_SRGB = 0x8C40;
    public const int GL_SRGB8 = 0x8C41;
    public const int GL_SRGB_ALPHA = 0x8C42;
    public const int GL_SRGB8_ALPHA8 = 0x8C43;
    public const int GL_COMPRESSED_SRGB = 0x8C48;
    public const int GL_COMPRESSED_SRGB_ALPHA = 0x8C49;
    public const int GL_COMPARE_REF_TO_TEXTURE = 0x884E;
    public const int GL_CLIP_DISTANCE0 = 0x3000;
    public const int GL_CLIP_DISTANCE1 = 0x3001;
    public const int GL_CLIP_DISTANCE2 = 0x3002;
    public const int GL_CLIP_DISTANCE3 = 0x3003;
    public const int GL_CLIP_DISTANCE4 = 0x3004;
    public const int GL_CLIP_DISTANCE5 = 0x3005;
    public const int GL_CLIP_DISTANCE6 = 0x3006;
    public const int GL_CLIP_DISTANCE7 = 0x3007;
    public const int GL_MAX_CLIP_DISTANCES = 0x0D32;
    public const int GL_MAJOR_VERSION = 0x821B;
    public const int GL_MINOR_VERSION = 0x821C;
    public const int GL_NUM_EXTENSIONS = 0x821D;
    public const int GL_CONTEXT_FLAGS = 0x821E;
    public const int GL_COMPRESSED_RED = 0x8225;
    public const int GL_COMPRESSED_RG = 0x8226;
    public const int GL_CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = 0x00000001;
    public const int GL_RGBA32F = 0x8814;
    public const int GL_RGB32F = 0x8815;
    public const int GL_RGBA16F = 0x881A;
    public const int GL_RGB16F = 0x881B;
    public const int GL_VERTEX_ATTRIB_ARRAY_INTEGER = 0x88FD;
    public const int GL_MAX_ARRAY_TEXTURE_LAYERS = 0x88FF;
    public const int GL_MIN_PROGRAM_TEXEL_OFFSET = 0x8904;
    public const int GL_MAX_PROGRAM_TEXEL_OFFSET = 0x8905;
    public const int GL_CLAMP_READ_COLOR = 0x891C;
    public const int GL_FIXED_ONLY = 0x891D;
    public const int GL_MAX_VARYING_COMPONENTS = 0x8B4B;
    public const int GL_TEXTURE_1D_ARRAY = 0x8C18;
    public const int GL_PROXY_TEXTURE_1D_ARRAY = 0x8C19;
    public const int GL_TEXTURE_2D_ARRAY = 0x8C1A;
    public const int GL_PROXY_TEXTURE_2D_ARRAY = 0x8C1B;
    public const int GL_TEXTURE_BINDING_1D_ARRAY = 0x8C1C;
    public const int GL_TEXTURE_BINDING_2D_ARRAY = 0x8C1D;
    public const int GL_R11F_G11F_B10F = 0x8C3A;
    public const int GL_UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B;
    public const int GL_RGB9_E5 = 0x8C3D;
    public const int GL_UNSIGNED_INT_5_9_9_9_REV = 0x8C3E;
    public const int GL_TEXTURE_SHARED_SIZE = 0x8C3F;
    public const int GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x8C76;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_MODE = 0x8C7F;
    public const int GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x8C80;
    public const int GL_TRANSFORM_FEEDBACK_VARYINGS = 0x8C83;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_START = 0x8C84;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x8C85;
    public const int GL_PRIMITIVES_GENERATED = 0x8C87;
    public const int GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x8C88;
    public const int GL_RASTERIZER_DISCARD = 0x8C89;
    public const int GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
    public const int GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x8C8B;
    public const int GL_INTERLEAVED_ATTRIBS = 0x8C8C;
    public const int GL_SEPARATE_ATTRIBS = 0x8C8D;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER = 0x8C8E;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x8C8F;
    public const int GL_RGBA32UI = 0x8D70;
    public const int GL_RGB32UI = 0x8D71;
    public const int GL_RGBA16UI = 0x8D76;
    public const int GL_RGB16UI = 0x8D77;
    public const int GL_RGBA8UI = 0x8D7C;
    public const int GL_RGB8UI = 0x8D7D;
    public const int GL_RGBA32I = 0x8D82;
    public const int GL_RGB32I = 0x8D83;
    public const int GL_RGBA16I = 0x8D88;
    public const int GL_RGB16I = 0x8D89;
    public const int GL_RGBA8I = 0x8D8E;
    public const int GL_RGB8I = 0x8D8F;
    public const int GL_RED_INTEGER = 0x8D94;
    public const int GL_GREEN_INTEGER = 0x8D95;
    public const int GL_BLUE_INTEGER = 0x8D96;
    public const int GL_RGB_INTEGER = 0x8D98;
    public const int GL_RGBA_INTEGER = 0x8D99;
    public const int GL_BGR_INTEGER = 0x8D9A;
    public const int GL_BGRA_INTEGER = 0x8D9B;
    public const int GL_SAMPLER_1D_ARRAY = 0x8DC0;
    public const int GL_SAMPLER_2D_ARRAY = 0x8DC1;
    public const int GL_SAMPLER_1D_ARRAY_SHADOW = 0x8DC3;
    public const int GL_SAMPLER_2D_ARRAY_SHADOW = 0x8DC4;
    public const int GL_SAMPLER_CUBE_SHADOW = 0x8DC5;
    public const int GL_UNSIGNED_INT_VEC2 = 0x8DC6;
    public const int GL_UNSIGNED_INT_VEC3 = 0x8DC7;
    public const int GL_UNSIGNED_INT_VEC4 = 0x8DC8;
    public const int GL_INT_SAMPLER_1D = 0x8DC9;
    public const int GL_INT_SAMPLER_2D = 0x8DCA;
    public const int GL_INT_SAMPLER_3D = 0x8DCB;
    public const int GL_INT_SAMPLER_CUBE = 0x8DCC;
    public const int GL_INT_SAMPLER_1D_ARRAY = 0x8DCE;
    public const int GL_INT_SAMPLER_2D_ARRAY = 0x8DCF;
    public const int GL_UNSIGNED_INT_SAMPLER_1D = 0x8DD1;
    public const int GL_UNSIGNED_INT_SAMPLER_2D = 0x8DD2;
    public const int GL_UNSIGNED_INT_SAMPLER_3D = 0x8DD3;
    public const int GL_UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4;
    public const int GL_UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x8DD6;
    public const int GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7;
    public const int GL_QUERY_WAIT = 0x8E13;
    public const int GL_QUERY_NO_WAIT = 0x8E14;
    public const int GL_QUERY_BY_REGION_WAIT = 0x8E15;
    public const int GL_QUERY_BY_REGION_NO_WAIT = 0x8E16;
    public const int GL_BUFFER_ACCESS_FLAGS = 0x911F;
    public const int GL_BUFFER_MAP_LENGTH = 0x9120;
    public const int GL_BUFFER_MAP_OFFSET = 0x9121;
    public const int GL_DEPTH_COMPONENT32F = 0x8CAC;
    public const int GL_DEPTH32F_STENCIL8 = 0x8CAD;
    public const int GL_FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD;
    public const int GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;
    public const int GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
    public const int GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
    public const int GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x8212;
    public const int GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x8213;
    public const int GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x8214;
    public const int GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x8215;
    public const int GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x8216;
    public const int GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x8217;
    public const int GL_FRAMEBUFFER_DEFAULT = 0x8218;
    public const int GL_FRAMEBUFFER_UNDEFINED = 0x8219;
    public const int GL_DEPTH_STENCIL_ATTACHMENT = 0x821A;
    public const int GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
    public const int GL_DEPTH_STENCIL = 0x84F9;
    public const int GL_UNSIGNED_INT_24_8 = 0x84FA;
    public const int GL_DEPTH24_STENCIL8 = 0x88F0;
    public const int GL_TEXTURE_STENCIL_SIZE = 0x88F1;
    public const int GL_TEXTURE_RED_TYPE = 0x8C10;
    public const int GL_TEXTURE_GREEN_TYPE = 0x8C11;
    public const int GL_TEXTURE_BLUE_TYPE = 0x8C12;
    public const int GL_TEXTURE_ALPHA_TYPE = 0x8C13;
    public const int GL_TEXTURE_DEPTH_TYPE = 0x8C16;
    public const int GL_UNSIGNED_NORMALIZED = 0x8C17;
    public const int GL_FRAMEBUFFER_BINDING = 0x8CA6;
    public const int GL_DRAW_FRAMEBUFFER_BINDING = 0x8CA6;
    public const int GL_RENDERBUFFER_BINDING = 0x8CA7;
    public const int GL_READ_FRAMEBUFFER = 0x8CA8;
    public const int GL_DRAW_FRAMEBUFFER = 0x8CA9;
    public const int GL_READ_FRAMEBUFFER_BINDING = 0x8CAA;
    public const int GL_RENDERBUFFER_SAMPLES = 0x8CAB;
    public const int GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
    public const int GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
    public const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
    public const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
    public const int GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x8CD4;
    public const int GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
    public const int GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
    public const int GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
    public const int GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x8CDB;
    public const int GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x8CDC;
    public const int GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
    public const int GL_MAX_COLOR_ATTACHMENTS = 0x8CDF;
    public const int GL_COLOR_ATTACHMENT0 = 0x8CE0;
    public const int GL_COLOR_ATTACHMENT1 = 0x8CE1;
    public const int GL_COLOR_ATTACHMENT2 = 0x8CE2;
    public const int GL_COLOR_ATTACHMENT3 = 0x8CE3;
    public const int GL_COLOR_ATTACHMENT4 = 0x8CE4;
    public const int GL_COLOR_ATTACHMENT5 = 0x8CE5;
    public const int GL_COLOR_ATTACHMENT6 = 0x8CE6;
    public const int GL_COLOR_ATTACHMENT7 = 0x8CE7;
    public const int GL_COLOR_ATTACHMENT8 = 0x8CE8;
    public const int GL_COLOR_ATTACHMENT9 = 0x8CE9;
    public const int GL_COLOR_ATTACHMENT10 = 0x8CEA;
    public const int GL_COLOR_ATTACHMENT11 = 0x8CEB;
    public const int GL_COLOR_ATTACHMENT12 = 0x8CEC;
    public const int GL_COLOR_ATTACHMENT13 = 0x8CED;
    public const int GL_COLOR_ATTACHMENT14 = 0x8CEE;
    public const int GL_COLOR_ATTACHMENT15 = 0x8CEF;
    public const int GL_COLOR_ATTACHMENT16 = 0x8CF0;
    public const int GL_COLOR_ATTACHMENT17 = 0x8CF1;
    public const int GL_COLOR_ATTACHMENT18 = 0x8CF2;
    public const int GL_COLOR_ATTACHMENT19 = 0x8CF3;
    public const int GL_COLOR_ATTACHMENT20 = 0x8CF4;
    public const int GL_COLOR_ATTACHMENT21 = 0x8CF5;
    public const int GL_COLOR_ATTACHMENT22 = 0x8CF6;
    public const int GL_COLOR_ATTACHMENT23 = 0x8CF7;
    public const int GL_COLOR_ATTACHMENT24 = 0x8CF8;
    public const int GL_COLOR_ATTACHMENT25 = 0x8CF9;
    public const int GL_COLOR_ATTACHMENT26 = 0x8CFA;
    public const int GL_COLOR_ATTACHMENT27 = 0x8CFB;
    public const int GL_COLOR_ATTACHMENT28 = 0x8CFC;
    public const int GL_COLOR_ATTACHMENT29 = 0x8CFD;
    public const int GL_COLOR_ATTACHMENT30 = 0x8CFE;
    public const int GL_COLOR_ATTACHMENT31 = 0x8CFF;
    public const int GL_DEPTH_ATTACHMENT = 0x8D00;
    public const int GL_STENCIL_ATTACHMENT = 0x8D20;
    public const int GL_FRAMEBUFFER = 0x8D40;
    public const int GL_RENDERBUFFER = 0x8D41;
    public const int GL_RENDERBUFFER_WIDTH = 0x8D42;
    public const int GL_RENDERBUFFER_HEIGHT = 0x8D43;
    public const int GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
    public const int GL_STENCIL_INDEX1 = 0x8D46;
    public const int GL_STENCIL_INDEX4 = 0x8D47;
    public const int GL_STENCIL_INDEX8 = 0x8D48;
    public const int GL_STENCIL_INDEX16 = 0x8D49;
    public const int GL_RENDERBUFFER_RED_SIZE = 0x8D50;
    public const int GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
    public const int GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
    public const int GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
    public const int GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
    public const int GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
    public const int GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
    public const int GL_MAX_SAMPLES = 0x8D57;
    public const int GL_FRAMEBUFFER_SRGB = 0x8DB9;
    public const int GL_HALF_FLOAT = 0x140B;
    public const int GL_MAP_READ_BIT = 0x0001;
    public const int GL_MAP_WRITE_BIT = 0x0002;
    public const int GL_MAP_INVALIDATE_RANGE_BIT = 0x0004;
    public const int GL_MAP_INVALIDATE_BUFFER_BIT = 0x0008;
    public const int GL_MAP_FLUSH_EXPLICIT_BIT = 0x0010;
    public const int GL_MAP_UNSYNCHRONIZED_BIT = 0x0020;
    public const int GL_COMPRESSED_RED_RGTC1 = 0x8DBB;
    public const int GL_COMPRESSED_SIGNED_RED_RGTC1 = 0x8DBC;
    public const int GL_COMPRESSED_RG_RGTC2 = 0x8DBD;
    public const int GL_COMPRESSED_SIGNED_RG_RGTC2 = 0x8DBE;
    public const int GL_RG = 0x8227;
    public const int GL_RG_INTEGER = 0x8228;
    public const int GL_R8 = 0x8229;
    public const int GL_R16 = 0x822A;
    public const int GL_RG8 = 0x822B;
    public const int GL_RG16 = 0x822C;
    public const int GL_R16F = 0x822D;
    public const int GL_R32F = 0x822E;
    public const int GL_RG16F = 0x822F;
    public const int GL_RG32F = 0x8230;
    public const int GL_R8I = 0x8231;
    public const int GL_R8UI = 0x8232;
    public const int GL_R16I = 0x8233;
    public const int GL_R16UI = 0x8234;
    public const int GL_R32I = 0x8235;
    public const int GL_R32UI = 0x8236;
    public const int GL_RG8I = 0x8237;
    public const int GL_RG8UI = 0x8238;
    public const int GL_RG16I = 0x8239;
    public const int GL_RG16UI = 0x823A;
    public const int GL_RG32I = 0x823B;
    public const int GL_RG32UI = 0x823C;
    public const int GL_VERTEX_ARRAY_BINDING = 0x85B5;
    public const int GL_SAMPLER_2D_RECT = 0x8B63;
    public const int GL_SAMPLER_2D_RECT_SHADOW = 0x8B64;
    public const int GL_SAMPLER_BUFFER = 0x8DC2;
    public const int GL_INT_SAMPLER_2D_RECT = 0x8DCD;
    public const int GL_INT_SAMPLER_BUFFER = 0x8DD0;
    public const int GL_UNSIGNED_INT_SAMPLER_2D_RECT = 0x8DD5;
    public const int GL_UNSIGNED_INT_SAMPLER_BUFFER = 0x8DD8;
    public const int GL_TEXTURE_BUFFER = 0x8C2A;
    public const int GL_MAX_TEXTURE_BUFFER_SIZE = 0x8C2B;
    public const int GL_TEXTURE_BINDING_BUFFER = 0x8C2C;
    public const int GL_TEXTURE_BUFFER_DATA_STORE_BINDING = 0x8C2D;
    public const int GL_TEXTURE_RECTANGLE = 0x84F5;
    public const int GL_TEXTURE_BINDING_RECTANGLE = 0x84F6;
    public const int GL_PROXY_TEXTURE_RECTANGLE = 0x84F7;
    public const int GL_MAX_RECTANGLE_TEXTURE_SIZE = 0x84F8;
    public const int GL_R8_SNORM = 0x8F94;
    public const int GL_RG8_SNORM = 0x8F95;
    public const int GL_RGB8_SNORM = 0x8F96;
    public const int GL_RGBA8_SNORM = 0x8F97;
    public const int GL_R16_SNORM = 0x8F98;
    public const int GL_RG16_SNORM = 0x8F99;
    public const int GL_RGB16_SNORM = 0x8F9A;
    public const int GL_RGBA16_SNORM = 0x8F9B;
    public const int GL_SIGNED_NORMALIZED = 0x8F9C;
    public const int GL_PRIMITIVE_RESTART = 0x8F9D;
    public const int GL_PRIMITIVE_RESTART_INDEX = 0x8F9E;
    public const int GL_COPY_READ_BUFFER = 0x8F36;
    public const int GL_COPY_WRITE_BUFFER = 0x8F37;
    public const int GL_UNIFORM_BUFFER = 0x8A11;
    public const int GL_UNIFORM_BUFFER_BINDING = 0x8A28;
    public const int GL_UNIFORM_BUFFER_START = 0x8A29;
    public const int GL_UNIFORM_BUFFER_SIZE = 0x8A2A;
    public const int GL_MAX_VERTEX_UNIFORM_BLOCKS = 0x8A2B;
    public const int GL_MAX_GEOMETRY_UNIFORM_BLOCKS = 0x8A2C;
    public const int GL_MAX_FRAGMENT_UNIFORM_BLOCKS = 0x8A2D;
    public const int GL_MAX_COMBINED_UNIFORM_BLOCKS = 0x8A2E;
    public const int GL_MAX_UNIFORM_BUFFER_BINDINGS = 0x8A2F;
    public const int GL_MAX_UNIFORM_BLOCK_SIZE = 0x8A30;
    public const int GL_MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS = 0x8A31;
    public const int GL_MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS = 0x8A32;
    public const int GL_MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS = 0x8A33;
    public const int GL_UNIFORM_BUFFER_OFFSET_ALIGNMENT = 0x8A34;
    public const int GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH = 0x8A35;
    public const int GL_ACTIVE_UNIFORM_BLOCKS = 0x8A36;
    public const int GL_UNIFORM_TYPE = 0x8A37;
    public const int GL_UNIFORM_SIZE = 0x8A38;
    public const int GL_UNIFORM_NAME_LENGTH = 0x8A39;
    public const int GL_UNIFORM_BLOCK_INDEX = 0x8A3A;
    public const int GL_UNIFORM_OFFSET = 0x8A3B;
    public const int GL_UNIFORM_ARRAY_STRIDE = 0x8A3C;
    public const int GL_UNIFORM_MATRIX_STRIDE = 0x8A3D;
    public const int GL_UNIFORM_IS_ROW_MAJOR = 0x8A3E;
    public const int GL_UNIFORM_BLOCK_BINDING = 0x8A3F;
    public const int GL_UNIFORM_BLOCK_DATA_SIZE = 0x8A40;
    public const int GL_UNIFORM_BLOCK_NAME_LENGTH = 0x8A41;
    public const int GL_UNIFORM_BLOCK_ACTIVE_UNIFORMS = 0x8A42;
    public const int GL_UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES = 0x8A43;
    public const int GL_UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER = 0x8A44;
    public const int GL_UNIFORM_BLOCK_REFERENCED_BY_GEOMETRY_SHADER = 0x8A45;
    public const int GL_UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER = 0x8A46;
    public const uint GL_INVALID_INDEX = 0xFFFFFFFF;
    public const int GL_CONTEXT_CORE_PROFILE_BIT = 0x00000001;
    public const int GL_CONTEXT_COMPATIBILITY_PROFILE_BIT = 0x00000002;
    public const int GL_LINES_ADJACENCY = 0x000A;
    public const int GL_LINE_STRIP_ADJACENCY = 0x000B;
    public const int GL_TRIANGLES_ADJACENCY = 0x000C;
    public const int GL_TRIANGLE_STRIP_ADJACENCY = 0x000D;
    public const int GL_PROGRAM_POINT_SIZE = 0x8642;
    public const int GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS = 0x8C29;
    public const int GL_FRAMEBUFFER_ATTACHMENT_LAYERED = 0x8DA7;
    public const int GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS = 0x8DA8;
    public const int GL_GEOMETRY_SHADER = 0x8DD9;
    public const int GL_GEOMETRY_VERTICES_OUT = 0x8916;
    public const int GL_GEOMETRY_INPUT_TYPE = 0x8917;
    public const int GL_GEOMETRY_OUTPUT_TYPE = 0x8918;
    public const int GL_MAX_GEOMETRY_UNIFORM_COMPONENTS = 0x8DDF;
    public const int GL_MAX_GEOMETRY_OUTPUT_VERTICES = 0x8DE0;
    public const int GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS = 0x8DE1;
    public const int GL_MAX_VERTEX_OUTPUT_COMPONENTS = 0x9122;
    public const int GL_MAX_GEOMETRY_INPUT_COMPONENTS = 0x9123;
    public const int GL_MAX_GEOMETRY_OUTPUT_COMPONENTS = 0x9124;
    public const int GL_MAX_FRAGMENT_INPUT_COMPONENTS = 0x9125;
    public const int GL_CONTEXT_PROFILE_MASK = 0x9126;
    public const int GL_DEPTH_CLAMP = 0x864F;
    public const int GL_QUADS_FOLLOW_PROVOKING_VERTEX_CONVENTION = 0x8E4C;
    public const int GL_FIRST_VERTEX_CONVENTION = 0x8E4D;
    public const int GL_LAST_VERTEX_CONVENTION = 0x8E4E;
    public const int GL_PROVOKING_VERTEX = 0x8E4F;
    public const int GL_TEXTURE_CUBE_MAP_SEAMLESS = 0x884F;
    public const int GL_MAX_SERVER_WAIT_TIMEOUT = 0x9111;
    public const int GL_OBJECT_TYPE = 0x9112;
    public const int GL_SYNC_CONDITION = 0x9113;
    public const int GL_SYNC_STATUS = 0x9114;
    public const int GL_SYNC_FLAGS = 0x9115;
    public const int GL_SYNC_FENCE = 0x9116;
    public const int GL_SYNC_GPU_COMMANDS_COMPLETE = 0x9117;
    public const int GL_UNSIGNALED = 0x9118;
    public const int GL_SIGNALED = 0x9119;
    public const int GL_ALREADY_SIGNALED = 0x911A;
    public const int GL_TIMEOUT_EXPIRED = 0x911B;
    public const int GL_CONDITION_SATISFIED = 0x911C;
    public const int GL_WAIT_FAILED = 0x911D;
    public const ulong GL_TIMEOUT_IGNORED = 0xFFFFFFFFFFFFFFFF;
    public const int GL_SYNC_FLUSH_COMMANDS_BIT = 0x00000001;
    public const int GL_SAMPLE_POSITION = 0x8E50;
    public const int GL_SAMPLE_MASK = 0x8E51;
    public const int GL_SAMPLE_MASK_VALUE = 0x8E52;
    public const int GL_MAX_SAMPLE_MASK_WORDS = 0x8E59;
    public const int GL_TEXTURE_2D_MULTISAMPLE = 0x9100;
    public const int GL_PROXY_TEXTURE_2D_MULTISAMPLE = 0x9101;
    public const int GL_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102;
    public const int GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9103;
    public const int GL_TEXTURE_BINDING_2D_MULTISAMPLE = 0x9104;
    public const int GL_TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY = 0x9105;
    public const int GL_TEXTURE_SAMPLES = 0x9106;
    public const int GL_TEXTURE_FIXED_SAMPLE_LOCATIONS = 0x9107;
    public const int GL_SAMPLER_2D_MULTISAMPLE = 0x9108;
    public const int GL_INT_SAMPLER_2D_MULTISAMPLE = 0x9109;
    public const int GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE = 0x910A;
    public const int GL_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910B;
    public const int GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910C;
    public const int GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910D;
    public const int GL_MAX_COLOR_TEXTURE_SAMPLES = 0x910E;
    public const int GL_MAX_DEPTH_TEXTURE_SAMPLES = 0x910F;
    public const int GL_MAX_INTEGER_SAMPLES = 0x9110;
    public const int GL_VERTEX_ATTRIB_ARRAY_DIVISOR = 0x88FE;
    public const int GL_SRC1_COLOR = 0x88F9;
    public const int GL_ONE_MINUS_SRC1_COLOR = 0x88FA;
    public const int GL_ONE_MINUS_SRC1_ALPHA = 0x88FB;
    public const int GL_MAX_DUAL_SOURCE_DRAW_BUFFERS = 0x88FC;
    public const int GL_ANY_SAMPLES_PASSED = 0x8C2F;
    public const int GL_SAMPLER_BINDING = 0x8919;
    public const int GL_RGB10_A2UI = 0x906F;
    public const int GL_TEXTURE_SWIZZLE_R = 0x8E42;
    public const int GL_TEXTURE_SWIZZLE_G = 0x8E43;
    public const int GL_TEXTURE_SWIZZLE_B = 0x8E44;
    public const int GL_TEXTURE_SWIZZLE_A = 0x8E45;
    public const int GL_TEXTURE_SWIZZLE_RGBA = 0x8E46;
    public const int GL_TIME_ELAPSED = 0x88BF;
    public const int GL_TIMESTAMP = 0x8E28;
    public const int GL_INT_2_10_10_10_REV = 0x8D9F;
    public const int GL_SAMPLE_SHADING = 0x8C36;
    public const int GL_MIN_SAMPLE_SHADING_VALUE = 0x8C37;
    public const int GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5E;
    public const int GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5F;
    public const int GL_TEXTURE_CUBE_MAP_ARRAY = 0x9009;
    public const int GL_TEXTURE_BINDING_CUBE_MAP_ARRAY = 0x900A;
    public const int GL_PROXY_TEXTURE_CUBE_MAP_ARRAY = 0x900B;
    public const int GL_SAMPLER_CUBE_MAP_ARRAY = 0x900C;
    public const int GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW = 0x900D;
    public const int GL_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900E;
    public const int GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900F;
    public const int GL_DRAW_INDIRECT_BUFFER = 0x8F3F;
    public const int GL_DRAW_INDIRECT_BUFFER_BINDING = 0x8F43;
    public const int GL_GEOMETRY_SHADER_INVOCATIONS = 0x887F;
    public const int GL_MAX_GEOMETRY_SHADER_INVOCATIONS = 0x8E5A;
    public const int GL_MIN_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5B;
    public const int GL_MAX_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5C;
    public const int GL_FRAGMENT_INTERPOLATION_OFFSET_BITS = 0x8E5D;
    public const int GL_MAX_VERTEX_STREAMS = 0x8E71;
    public const int GL_DOUBLE_VEC2 = 0x8FFC;
    public const int GL_DOUBLE_VEC3 = 0x8FFD;
    public const int GL_DOUBLE_VEC4 = 0x8FFE;
    public const int GL_DOUBLE_MAT2 = 0x8F46;
    public const int GL_DOUBLE_MAT3 = 0x8F47;
    public const int GL_DOUBLE_MAT4 = 0x8F48;
    public const int GL_DOUBLE_MAT2x3 = 0x8F49;
    public const int GL_DOUBLE_MAT2x4 = 0x8F4A;
    public const int GL_DOUBLE_MAT3x2 = 0x8F4B;
    public const int GL_DOUBLE_MAT3x4 = 0x8F4C;
    public const int GL_DOUBLE_MAT4x2 = 0x8F4D;
    public const int GL_DOUBLE_MAT4x3 = 0x8F4E;
    public const int GL_ACTIVE_SUBROUTINES = 0x8DE5;
    public const int GL_ACTIVE_SUBROUTINE_UNIFORMS = 0x8DE6;
    public const int GL_ACTIVE_SUBROUTINE_UNIFORM_LOCATIONS = 0x8E47;
    public const int GL_ACTIVE_SUBROUTINE_MAX_LENGTH = 0x8E48;
    public const int GL_ACTIVE_SUBROUTINE_UNIFORM_MAX_LENGTH = 0x8E49;
    public const int GL_MAX_SUBROUTINES = 0x8DE7;
    public const int GL_MAX_SUBROUTINE_UNIFORM_LOCATIONS = 0x8DE8;
    public const int GL_NUM_COMPATIBLE_SUBROUTINES = 0x8E4A;
    public const int GL_COMPATIBLE_SUBROUTINES = 0x8E4B;
    public const int GL_PATCHES = 0x000E;
    public const int GL_PATCH_VERTICES = 0x8E72;
    public const int GL_PATCH_DEFAULT_INNER_LEVEL = 0x8E73;
    public const int GL_PATCH_DEFAULT_OUTER_LEVEL = 0x8E74;
    public const int GL_TESS_CONTROL_OUTPUT_VERTICES = 0x8E75;
    public const int GL_TESS_GEN_MODE = 0x8E76;
    public const int GL_TESS_GEN_SPACING = 0x8E77;
    public const int GL_TESS_GEN_VERTEX_ORDER = 0x8E78;
    public const int GL_TESS_GEN_POINT_MODE = 0x8E79;
    public const int GL_ISOLINES = 0x8E7A;
    public const int GL_QUADS = 0x0007;
    public const int GL_FRACTIONAL_ODD = 0x8E7B;
    public const int GL_FRACTIONAL_EVEN = 0x8E7C;
    public const int GL_MAX_PATCH_VERTICES = 0x8E7D;
    public const int GL_MAX_TESS_GEN_LEVEL = 0x8E7E;
    public const int GL_MAX_TESS_CONTROL_UNIFORM_COMPONENTS = 0x8E7F;
    public const int GL_MAX_TESS_EVALUATION_UNIFORM_COMPONENTS = 0x8E80;
    public const int GL_MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS = 0x8E81;
    public const int GL_MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS = 0x8E82;
    public const int GL_MAX_TESS_CONTROL_OUTPUT_COMPONENTS = 0x8E83;
    public const int GL_MAX_TESS_PATCH_COMPONENTS = 0x8E84;
    public const int GL_MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS = 0x8E85;
    public const int GL_MAX_TESS_EVALUATION_OUTPUT_COMPONENTS = 0x8E86;
    public const int GL_MAX_TESS_CONTROL_UNIFORM_BLOCKS = 0x8E89;
    public const int GL_MAX_TESS_EVALUATION_UNIFORM_BLOCKS = 0x8E8A;
    public const int GL_MAX_TESS_CONTROL_INPUT_COMPONENTS = 0x886C;
    public const int GL_MAX_TESS_EVALUATION_INPUT_COMPONENTS = 0x886D;
    public const int GL_MAX_COMBINED_TESS_CONTROL_UNIFORM_COMPONENTS = 0x8E1E;
    public const int GL_MAX_COMBINED_TESS_EVALUATION_UNIFORM_COMPONENTS = 0x8E1F;
    public const int GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_CONTROL_SHADER = 0x84F0;
    public const int GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x84F1;
    public const int GL_TESS_EVALUATION_SHADER = 0x8E87;
    public const int GL_TESS_CONTROL_SHADER = 0x8E88;
    public const int GL_TRANSFORM_FEEDBACK = 0x8E22;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_PAUSED = 0x8E23;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_ACTIVE = 0x8E24;
    public const int GL_TRANSFORM_FEEDBACK_BINDING = 0x8E25;
    public const int GL_MAX_TRANSFORM_FEEDBACK_BUFFERS = 0x8E70;
    public const int GL_FIXED = 0x140C;
    public const int GL_IMPLEMENTATION_COLOR_READ_TYPE = 0x8B9A;
    public const int GL_IMPLEMENTATION_COLOR_READ_FORMAT = 0x8B9B;
    public const int GL_LOW_FLOAT = 0x8DF0;
    public const int GL_MEDIUM_FLOAT = 0x8DF1;
    public const int GL_HIGH_FLOAT = 0x8DF2;
    public const int GL_LOW_INT = 0x8DF3;
    public const int GL_MEDIUM_INT = 0x8DF4;
    public const int GL_HIGH_INT = 0x8DF5;
    public const int GL_SHADER_COMPILER = 0x8DFA;
    public const int GL_SHADER_BINARY_FORMATS = 0x8DF8;
    public const int GL_NUM_SHADER_BINARY_FORMATS = 0x8DF9;
    public const int GL_MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
    public const int GL_MAX_VARYING_VECTORS = 0x8DFC;
    public const int GL_MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
    public const int GL_RGB565 = 0x8D62;
    public const int GL_PROGRAM_BINARY_RETRIEVABLE_HINT = 0x8257;
    public const int GL_PROGRAM_BINARY_LENGTH = 0x8741;
    public const int GL_NUM_PROGRAM_BINARY_FORMATS = 0x87FE;
    public const int GL_PROGRAM_BINARY_FORMATS = 0x87FF;
    public const int GL_VERTEX_SHADER_BIT = 0x00000001;
    public const int GL_FRAGMENT_SHADER_BIT = 0x00000002;
    public const int GL_GEOMETRY_SHADER_BIT = 0x00000004;
    public const int GL_TESS_CONTROL_SHADER_BIT = 0x00000008;
    public const int GL_TESS_EVALUATION_SHADER_BIT = 0x00000010;
    public const uint GL_ALL_SHADER_BITS = 0xFFFFFFFF;
    public const int GL_PROGRAM_SEPARABLE = 0x8258;
    public const int GL_ACTIVE_PROGRAM = 0x8259;
    public const int GL_PROGRAM_PIPELINE_BINDING = 0x825A;
    public const int GL_MAX_VIEWPORTS = 0x825B;
    public const int GL_VIEWPORT_SUBPIXEL_BITS = 0x825C;
    public const int GL_VIEWPORT_BOUNDS_RANGE = 0x825D;
    public const int GL_LAYER_PROVOKING_VERTEX = 0x825E;
    public const int GL_VIEWPORT_INDEX_PROVOKING_VERTEX = 0x825F;
    public const int GL_UNDEFINED_VERTEX = 0x8260;
    public const int GL_COPY_READ_BUFFER_BINDING = 0x8F36;
    public const int GL_COPY_WRITE_BUFFER_BINDING = 0x8F37;
    public const int GL_TRANSFORM_FEEDBACK_ACTIVE = 0x8E24;
    public const int GL_TRANSFORM_FEEDBACK_PAUSED = 0x8E23;
    public const int GL_UNPACK_COMPRESSED_BLOCK_WIDTH = 0x9127;
    public const int GL_UNPACK_COMPRESSED_BLOCK_HEIGHT = 0x9128;
    public const int GL_UNPACK_COMPRESSED_BLOCK_DEPTH = 0x9129;
    public const int GL_UNPACK_COMPRESSED_BLOCK_SIZE = 0x912A;
    public const int GL_PACK_COMPRESSED_BLOCK_WIDTH = 0x912B;
    public const int GL_PACK_COMPRESSED_BLOCK_HEIGHT = 0x912C;
    public const int GL_PACK_COMPRESSED_BLOCK_DEPTH = 0x912D;
    public const int GL_PACK_COMPRESSED_BLOCK_SIZE = 0x912E;
    public const int GL_NUM_SAMPLE_COUNTS = 0x9380;
    public const int GL_MIN_MAP_BUFFER_ALIGNMENT = 0x90BC;
    public const int GL_ATOMIC_COUNTER_BUFFER = 0x92C0;
    public const int GL_ATOMIC_COUNTER_BUFFER_BINDING = 0x92C1;
    public const int GL_ATOMIC_COUNTER_BUFFER_START = 0x92C2;
    public const int GL_ATOMIC_COUNTER_BUFFER_SIZE = 0x92C3;
    public const int GL_ATOMIC_COUNTER_BUFFER_DATA_SIZE = 0x92C4;
    public const int GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTERS = 0x92C5;
    public const int GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTER_INDICES = 0x92C6;
    public const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_VERTEX_SHADER = 0x92C7;
    public const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_CONTROL_SHADER = 0x92C8;
    public const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x92C9;
    public const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_GEOMETRY_SHADER = 0x92CA;
    public const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_FRAGMENT_SHADER = 0x92CB;
    public const int GL_MAX_VERTEX_ATOMIC_COUNTER_BUFFERS = 0x92CC;
    public const int GL_MAX_TESS_CONTROL_ATOMIC_COUNTER_BUFFERS = 0x92CD;
    public const int GL_MAX_TESS_EVALUATION_ATOMIC_COUNTER_BUFFERS = 0x92CE;
    public const int GL_MAX_GEOMETRY_ATOMIC_COUNTER_BUFFERS = 0x92CF;
    public const int GL_MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS = 0x92D0;
    public const int GL_MAX_COMBINED_ATOMIC_COUNTER_BUFFERS = 0x92D1;
    public const int GL_MAX_VERTEX_ATOMIC_COUNTERS = 0x92D2;
    public const int GL_MAX_TESS_CONTROL_ATOMIC_COUNTERS = 0x92D3;
    public const int GL_MAX_TESS_EVALUATION_ATOMIC_COUNTERS = 0x92D4;
    public const int GL_MAX_GEOMETRY_ATOMIC_COUNTERS = 0x92D5;
    public const int GL_MAX_FRAGMENT_ATOMIC_COUNTERS = 0x92D6;
    public const int GL_MAX_COMBINED_ATOMIC_COUNTERS = 0x92D7;
    public const int GL_MAX_ATOMIC_COUNTER_BUFFER_SIZE = 0x92D8;
    public const int GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS = 0x92DC;
    public const int GL_ACTIVE_ATOMIC_COUNTER_BUFFERS = 0x92D9;
    public const int GL_UNIFORM_ATOMIC_COUNTER_BUFFER_INDEX = 0x92DA;
    public const int GL_UNSIGNED_INT_ATOMIC_COUNTER = 0x92DB;
    public const int GL_VERTEX_ATTRIB_ARRAY_BARRIER_BIT = 0x00000001;
    public const int GL_ELEMENT_ARRAY_BARRIER_BIT = 0x00000002;
    public const int GL_UNIFORM_BARRIER_BIT = 0x00000004;
    public const int GL_TEXTURE_FETCH_BARRIER_BIT = 0x00000008;
    public const int GL_SHADER_IMAGE_ACCESS_BARRIER_BIT = 0x00000020;
    public const int GL_COMMAND_BARRIER_BIT = 0x00000040;
    public const int GL_PIXEL_BUFFER_BARRIER_BIT = 0x00000080;
    public const int GL_TEXTURE_UPDATE_BARRIER_BIT = 0x00000100;
    public const int GL_BUFFER_UPDATE_BARRIER_BIT = 0x00000200;
    public const int GL_FRAMEBUFFER_BARRIER_BIT = 0x00000400;
    public const int GL_TRANSFORM_FEEDBACK_BARRIER_BIT = 0x00000800;
    public const int GL_ATOMIC_COUNTER_BARRIER_BIT = 0x00001000;
    public const uint GL_ALL_BARRIER_BITS = 0xFFFFFFFF;
    public const int GL_MAX_IMAGE_UNITS = 0x8F38;
    public const int GL_MAX_COMBINED_IMAGE_UNITS_AND_FRAGMENT_OUTPUTS = 0x8F39;
    public const int GL_IMAGE_BINDING_NAME = 0x8F3A;
    public const int GL_IMAGE_BINDING_LEVEL = 0x8F3B;
    public const int GL_IMAGE_BINDING_LAYERED = 0x8F3C;
    public const int GL_IMAGE_BINDING_LAYER = 0x8F3D;
    public const int GL_IMAGE_BINDING_ACCESS = 0x8F3E;
    public const int GL_IMAGE_1D = 0x904C;
    public const int GL_IMAGE_2D = 0x904D;
    public const int GL_IMAGE_3D = 0x904E;
    public const int GL_IMAGE_2D_RECT = 0x904F;
    public const int GL_IMAGE_CUBE = 0x9050;
    public const int GL_IMAGE_BUFFER = 0x9051;
    public const int GL_IMAGE_1D_ARRAY = 0x9052;
    public const int GL_IMAGE_2D_ARRAY = 0x9053;
    public const int GL_IMAGE_CUBE_MAP_ARRAY = 0x9054;
    public const int GL_IMAGE_2D_MULTISAMPLE = 0x9055;
    public const int GL_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9056;
    public const int GL_INT_IMAGE_1D = 0x9057;
    public const int GL_INT_IMAGE_2D = 0x9058;
    public const int GL_INT_IMAGE_3D = 0x9059;
    public const int GL_INT_IMAGE_2D_RECT = 0x905A;
    public const int GL_INT_IMAGE_CUBE = 0x905B;
    public const int GL_INT_IMAGE_BUFFER = 0x905C;
    public const int GL_INT_IMAGE_1D_ARRAY = 0x905D;
    public const int GL_INT_IMAGE_2D_ARRAY = 0x905E;
    public const int GL_INT_IMAGE_CUBE_MAP_ARRAY = 0x905F;
    public const int GL_INT_IMAGE_2D_MULTISAMPLE = 0x9060;
    public const int GL_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9061;
    public const int GL_UNSIGNED_INT_IMAGE_1D = 0x9062;
    public const int GL_UNSIGNED_INT_IMAGE_2D = 0x9063;
    public const int GL_UNSIGNED_INT_IMAGE_3D = 0x9064;
    public const int GL_UNSIGNED_INT_IMAGE_2D_RECT = 0x9065;
    public const int GL_UNSIGNED_INT_IMAGE_CUBE = 0x9066;
    public const int GL_UNSIGNED_INT_IMAGE_BUFFER = 0x9067;
    public const int GL_UNSIGNED_INT_IMAGE_1D_ARRAY = 0x9068;
    public const int GL_UNSIGNED_INT_IMAGE_2D_ARRAY = 0x9069;
    public const int GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY = 0x906A;
    public const int GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE = 0x906B;
    public const int GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x906C;
    public const int GL_MAX_IMAGE_SAMPLES = 0x906D;
    public const int GL_IMAGE_BINDING_FORMAT = 0x906E;
    public const int GL_IMAGE_FORMAT_COMPATIBILITY_TYPE = 0x90C7;
    public const int GL_IMAGE_FORMAT_COMPATIBILITY_BY_SIZE = 0x90C8;
    public const int GL_IMAGE_FORMAT_COMPATIBILITY_BY_CLASS = 0x90C9;
    public const int GL_MAX_VERTEX_IMAGE_UNIFORMS = 0x90CA;
    public const int GL_MAX_TESS_CONTROL_IMAGE_UNIFORMS = 0x90CB;
    public const int GL_MAX_TESS_EVALUATION_IMAGE_UNIFORMS = 0x90CC;
    public const int GL_MAX_GEOMETRY_IMAGE_UNIFORMS = 0x90CD;
    public const int GL_MAX_FRAGMENT_IMAGE_UNIFORMS = 0x90CE;
    public const int GL_MAX_COMBINED_IMAGE_UNIFORMS = 0x90CF;
    public const int GL_COMPRESSED_RGBA_BPTC_UNORM = 0x8E8C;
    public const int GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM = 0x8E8D;
    public const int GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT = 0x8E8E;
    public const int GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT = 0x8E8F;
    public const int GL_TEXTURE_IMMUTABLE_FORMAT = 0x912F;
    public const int GL_NUM_SHADING_LANGUAGE_VERSIONS = 0x82E9;
    public const int GL_VERTEX_ATTRIB_ARRAY_LONG = 0x874E;
    public const int GL_COMPRESSED_RGB8_ETC2 = 0x9274;
    public const int GL_COMPRESSED_SRGB8_ETC2 = 0x9275;
    public const int GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9276;
    public const int GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277;
    public const int GL_COMPRESSED_RGBA8_ETC2_EAC = 0x9278;
    public const int GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC = 0x9279;
    public const int GL_COMPRESSED_R11_EAC = 0x9270;
    public const int GL_COMPRESSED_SIGNED_R11_EAC = 0x9271;
    public const int GL_COMPRESSED_RG11_EAC = 0x9272;
    public const int GL_COMPRESSED_SIGNED_RG11_EAC = 0x9273;
    public const int GL_PRIMITIVE_RESTART_FIXED_INDEX = 0x8D69;
    public const int GL_ANY_SAMPLES_PASSED_CONSERVATIVE = 0x8D6A;
    public const int GL_MAX_ELEMENT_INDEX = 0x8D6B;
    public const int GL_COMPUTE_SHADER = 0x91B9;
    public const int GL_MAX_COMPUTE_UNIFORM_BLOCKS = 0x91BB;
    public const int GL_MAX_COMPUTE_TEXTURE_IMAGE_UNITS = 0x91BC;
    public const int GL_MAX_COMPUTE_IMAGE_UNIFORMS = 0x91BD;
    public const int GL_MAX_COMPUTE_SHARED_MEMORY_SIZE = 0x8262;
    public const int GL_MAX_COMPUTE_UNIFORM_COMPONENTS = 0x8263;
    public const int GL_MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS = 0x8264;
    public const int GL_MAX_COMPUTE_ATOMIC_COUNTERS = 0x8265;
    public const int GL_MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS = 0x8266;
    public const int GL_MAX_COMPUTE_WORK_GROUP_INVOCATIONS = 0x90EB;
    public const int GL_MAX_COMPUTE_WORK_GROUP_COUNT = 0x91BE;
    public const int GL_MAX_COMPUTE_WORK_GROUP_SIZE = 0x91BF;
    public const int GL_COMPUTE_WORK_GROUP_SIZE = 0x8267;
    public const int GL_UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER = 0x90EC;
    public const int GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER = 0x90ED;
    public const int GL_DISPATCH_INDIRECT_BUFFER = 0x90EE;
    public const int GL_DISPATCH_INDIRECT_BUFFER_BINDING = 0x90EF;
    public const int GL_COMPUTE_SHADER_BIT = 0x00000020;
    public const int GL_DEBUG_OUTPUT_SYNCHRONOUS = 0x8242;
    public const int GL_DEBUG_NEXT_LOGGED_MESSAGE_LENGTH = 0x8243;
    public const int GL_DEBUG_CALLBACK_FUNCTION = 0x8244;
    public const int GL_DEBUG_CALLBACK_USER_PARAM = 0x8245;
    public const int GL_DEBUG_SOURCE_API = 0x8246;
    public const int GL_DEBUG_SOURCE_WINDOW_SYSTEM = 0x8247;
    public const int GL_DEBUG_SOURCE_SHADER_COMPILER = 0x8248;
    public const int GL_DEBUG_SOURCE_THIRD_PARTY = 0x8249;
    public const int GL_DEBUG_SOURCE_APPLICATION = 0x824A;
    public const int GL_DEBUG_SOURCE_OTHER = 0x824B;
    public const int GL_DEBUG_TYPE_ERROR = 0x824C;
    public const int GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR = 0x824D;
    public const int GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR = 0x824E;
    public const int GL_DEBUG_TYPE_PORTABILITY = 0x824F;
    public const int GL_DEBUG_TYPE_PERFORMANCE = 0x8250;
    public const int GL_DEBUG_TYPE_OTHER = 0x8251;
    public const int GL_MAX_DEBUG_MESSAGE_LENGTH = 0x9143;
    public const int GL_MAX_DEBUG_LOGGED_MESSAGES = 0x9144;
    public const int GL_DEBUG_LOGGED_MESSAGES = 0x9145;
    public const int GL_DEBUG_SEVERITY_HIGH = 0x9146;
    public const int GL_DEBUG_SEVERITY_MEDIUM = 0x9147;
    public const int GL_DEBUG_SEVERITY_LOW = 0x9148;
    public const int GL_DEBUG_TYPE_MARKER = 0x8268;
    public const int GL_DEBUG_TYPE_PUSH_GROUP = 0x8269;
    public const int GL_DEBUG_TYPE_POP_GROUP = 0x826A;
    public const int GL_DEBUG_SEVERITY_NOTIFICATION = 0x826B;
    public const int GL_MAX_DEBUG_GROUP_STACK_DEPTH = 0x826C;
    public const int GL_DEBUG_GROUP_STACK_DEPTH = 0x826D;
    public const int GL_BUFFER = 0x82E0;
    public const int GL_SHADER = 0x82E1;
    public const int GL_PROGRAM = 0x82E2;
    public const int GL_VERTEX_ARRAY = 0x8074;
    public const int GL_QUERY = 0x82E3;
    public const int GL_PROGRAM_PIPELINE = 0x82E4;
    public const int GL_SAMPLER = 0x82E6;
    public const int GL_MAX_LABEL_LENGTH = 0x82E8;
    public const int GL_DEBUG_OUTPUT = 0x92E0;
    public const int GL_CONTEXT_FLAG_DEBUG_BIT = 0x00000002;
    public const int GL_MAX_UNIFORM_LOCATIONS = 0x826E;
    public const int GL_FRAMEBUFFER_DEFAULT_WIDTH = 0x9310;
    public const int GL_FRAMEBUFFER_DEFAULT_HEIGHT = 0x9311;
    public const int GL_FRAMEBUFFER_DEFAULT_LAYERS = 0x9312;
    public const int GL_FRAMEBUFFER_DEFAULT_SAMPLES = 0x9313;
    public const int GL_FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS = 0x9314;
    public const int GL_MAX_FRAMEBUFFER_WIDTH = 0x9315;
    public const int GL_MAX_FRAMEBUFFER_HEIGHT = 0x9316;
    public const int GL_MAX_FRAMEBUFFER_LAYERS = 0x9317;
    public const int GL_MAX_FRAMEBUFFER_SAMPLES = 0x9318;
    public const int GL_INTERNALFORMAT_SUPPORTED = 0x826F;
    public const int GL_INTERNALFORMAT_PREFERRED = 0x8270;
    public const int GL_INTERNALFORMAT_RED_SIZE = 0x8271;
    public const int GL_INTERNALFORMAT_GREEN_SIZE = 0x8272;
    public const int GL_INTERNALFORMAT_BLUE_SIZE = 0x8273;
    public const int GL_INTERNALFORMAT_ALPHA_SIZE = 0x8274;
    public const int GL_INTERNALFORMAT_DEPTH_SIZE = 0x8275;
    public const int GL_INTERNALFORMAT_STENCIL_SIZE = 0x8276;
    public const int GL_INTERNALFORMAT_SHARED_SIZE = 0x8277;
    public const int GL_INTERNALFORMAT_RED_TYPE = 0x8278;
    public const int GL_INTERNALFORMAT_GREEN_TYPE = 0x8279;
    public const int GL_INTERNALFORMAT_BLUE_TYPE = 0x827A;
    public const int GL_INTERNALFORMAT_ALPHA_TYPE = 0x827B;
    public const int GL_INTERNALFORMAT_DEPTH_TYPE = 0x827C;
    public const int GL_INTERNALFORMAT_STENCIL_TYPE = 0x827D;
    public const int GL_MAX_WIDTH = 0x827E;
    public const int GL_MAX_HEIGHT = 0x827F;
    public const int GL_MAX_DEPTH = 0x8280;
    public const int GL_MAX_LAYERS = 0x8281;
    public const int GL_MAX_COMBINED_DIMENSIONS = 0x8282;
    public const int GL_COLOR_COMPONENTS = 0x8283;
    public const int GL_DEPTH_COMPONENTS = 0x8284;
    public const int GL_STENCIL_COMPONENTS = 0x8285;
    public const int GL_COLOR_RENDERABLE = 0x8286;
    public const int GL_DEPTH_RENDERABLE = 0x8287;
    public const int GL_STENCIL_RENDERABLE = 0x8288;
    public const int GL_FRAMEBUFFER_RENDERABLE = 0x8289;
    public const int GL_FRAMEBUFFER_RENDERABLE_LAYERED = 0x828A;
    public const int GL_FRAMEBUFFER_BLEND = 0x828B;
    public const int GL_READ_PIXELS = 0x828C;
    public const int GL_READ_PIXELS_FORMAT = 0x828D;
    public const int GL_READ_PIXELS_TYPE = 0x828E;
    public const int GL_TEXTURE_IMAGE_FORMAT = 0x828F;
    public const int GL_TEXTURE_IMAGE_TYPE = 0x8290;
    public const int GL_GET_TEXTURE_IMAGE_FORMAT = 0x8291;
    public const int GL_GET_TEXTURE_IMAGE_TYPE = 0x8292;
    public const int GL_MIPMAP = 0x8293;
    public const int GL_MANUAL_GENERATE_MIPMAP = 0x8294;
    public const int GL_AUTO_GENERATE_MIPMAP = 0x8295;
    public const int GL_COLOR_ENCODING = 0x8296;
    public const int GL_SRGB_READ = 0x8297;
    public const int GL_SRGB_WRITE = 0x8298;
    public const int GL_FILTER = 0x829A;
    public const int GL_VERTEX_TEXTURE = 0x829B;
    public const int GL_TESS_CONTROL_TEXTURE = 0x829C;
    public const int GL_TESS_EVALUATION_TEXTURE = 0x829D;
    public const int GL_GEOMETRY_TEXTURE = 0x829E;
    public const int GL_FRAGMENT_TEXTURE = 0x829F;
    public const int GL_COMPUTE_TEXTURE = 0x82A0;
    public const int GL_TEXTURE_SHADOW = 0x82A1;
    public const int GL_TEXTURE_GATHER = 0x82A2;
    public const int GL_TEXTURE_GATHER_SHADOW = 0x82A3;
    public const int GL_SHADER_IMAGE_LOAD = 0x82A4;
    public const int GL_SHADER_IMAGE_STORE = 0x82A5;
    public const int GL_SHADER_IMAGE_ATOMIC = 0x82A6;
    public const int GL_IMAGE_TEXEL_SIZE = 0x82A7;
    public const int GL_IMAGE_COMPATIBILITY_CLASS = 0x82A8;
    public const int GL_IMAGE_PIXEL_FORMAT = 0x82A9;
    public const int GL_IMAGE_PIXEL_TYPE = 0x82AA;
    public const int GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST = 0x82AC;
    public const int GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST = 0x82AD;
    public const int GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE = 0x82AE;
    public const int GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE = 0x82AF;
    public const int GL_TEXTURE_COMPRESSED_BLOCK_WIDTH = 0x82B1;
    public const int GL_TEXTURE_COMPRESSED_BLOCK_HEIGHT = 0x82B2;
    public const int GL_TEXTURE_COMPRESSED_BLOCK_SIZE = 0x82B3;
    public const int GL_CLEAR_BUFFER = 0x82B4;
    public const int GL_TEXTURE_VIEW = 0x82B5;
    public const int GL_VIEW_COMPATIBILITY_CLASS = 0x82B6;
    public const int GL_FULL_SUPPORT = 0x82B7;
    public const int GL_CAVEAT_SUPPORT = 0x82B8;
    public const int GL_IMAGE_CLASS_4_X_32 = 0x82B9;
    public const int GL_IMAGE_CLASS_2_X_32 = 0x82BA;
    public const int GL_IMAGE_CLASS_1_X_32 = 0x82BB;
    public const int GL_IMAGE_CLASS_4_X_16 = 0x82BC;
    public const int GL_IMAGE_CLASS_2_X_16 = 0x82BD;
    public const int GL_IMAGE_CLASS_1_X_16 = 0x82BE;
    public const int GL_IMAGE_CLASS_4_X_8 = 0x82BF;
    public const int GL_IMAGE_CLASS_2_X_8 = 0x82C0;
    public const int GL_IMAGE_CLASS_1_X_8 = 0x82C1;
    public const int GL_IMAGE_CLASS_11_11_10 = 0x82C2;
    public const int GL_IMAGE_CLASS_10_10_10_2 = 0x82C3;
    public const int GL_VIEW_CLASS_128_BITS = 0x82C4;
    public const int GL_VIEW_CLASS_96_BITS = 0x82C5;
    public const int GL_VIEW_CLASS_64_BITS = 0x82C6;
    public const int GL_VIEW_CLASS_48_BITS = 0x82C7;
    public const int GL_VIEW_CLASS_32_BITS = 0x82C8;
    public const int GL_VIEW_CLASS_24_BITS = 0x82C9;
    public const int GL_VIEW_CLASS_16_BITS = 0x82CA;
    public const int GL_VIEW_CLASS_8_BITS = 0x82CB;
    public const int GL_VIEW_CLASS_S3TC_DXT1_RGB = 0x82CC;
    public const int GL_VIEW_CLASS_S3TC_DXT1_RGBA = 0x82CD;
    public const int GL_VIEW_CLASS_S3TC_DXT3_RGBA = 0x82CE;
    public const int GL_VIEW_CLASS_S3TC_DXT5_RGBA = 0x82CF;
    public const int GL_VIEW_CLASS_RGTC1_RED = 0x82D0;
    public const int GL_VIEW_CLASS_RGTC2_RG = 0x82D1;
    public const int GL_VIEW_CLASS_BPTC_UNORM = 0x82D2;
    public const int GL_VIEW_CLASS_BPTC_FLOAT = 0x82D3;
    public const int GL_UNIFORM = 0x92E1;
    public const int GL_UNIFORM_BLOCK = 0x92E2;
    public const int GL_PROGRAM_INPUT = 0x92E3;
    public const int GL_PROGRAM_OUTPUT = 0x92E4;
    public const int GL_BUFFER_VARIABLE = 0x92E5;
    public const int GL_SHADER_STORAGE_BLOCK = 0x92E6;
    public const int GL_VERTEX_SUBROUTINE = 0x92E8;
    public const int GL_TESS_CONTROL_SUBROUTINE = 0x92E9;
    public const int GL_TESS_EVALUATION_SUBROUTINE = 0x92EA;
    public const int GL_GEOMETRY_SUBROUTINE = 0x92EB;
    public const int GL_FRAGMENT_SUBROUTINE = 0x92EC;
    public const int GL_COMPUTE_SUBROUTINE = 0x92ED;
    public const int GL_VERTEX_SUBROUTINE_UNIFORM = 0x92EE;
    public const int GL_TESS_CONTROL_SUBROUTINE_UNIFORM = 0x92EF;
    public const int GL_TESS_EVALUATION_SUBROUTINE_UNIFORM = 0x92F0;
    public const int GL_GEOMETRY_SUBROUTINE_UNIFORM = 0x92F1;
    public const int GL_FRAGMENT_SUBROUTINE_UNIFORM = 0x92F2;
    public const int GL_COMPUTE_SUBROUTINE_UNIFORM = 0x92F3;
    public const int GL_TRANSFORM_FEEDBACK_VARYING = 0x92F4;
    public const int GL_ACTIVE_RESOURCES = 0x92F5;
    public const int GL_MAX_NAME_LENGTH = 0x92F6;
    public const int GL_MAX_NUM_ACTIVE_VARIABLES = 0x92F7;
    public const int GL_MAX_NUM_COMPATIBLE_SUBROUTINES = 0x92F8;
    public const int GL_NAME_LENGTH = 0x92F9;
    public const int GL_TYPE = 0x92FA;
    public const int GL_ARRAY_SIZE = 0x92FB;
    public const int GL_OFFSET = 0x92FC;
    public const int GL_BLOCK_INDEX = 0x92FD;
    public const int GL_ARRAY_STRIDE = 0x92FE;
    public const int GL_MATRIX_STRIDE = 0x92FF;
    public const int GL_IS_ROW_MAJOR = 0x9300;
    public const int GL_ATOMIC_COUNTER_BUFFER_INDEX = 0x9301;
    public const int GL_BUFFER_BINDING = 0x9302;
    public const int GL_BUFFER_DATA_SIZE = 0x9303;
    public const int GL_NUM_ACTIVE_VARIABLES = 0x9304;
    public const int GL_ACTIVE_VARIABLES = 0x9305;
    public const int GL_REFERENCED_BY_VERTEX_SHADER = 0x9306;
    public const int GL_REFERENCED_BY_TESS_CONTROL_SHADER = 0x9307;
    public const int GL_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x9308;
    public const int GL_REFERENCED_BY_GEOMETRY_SHADER = 0x9309;
    public const int GL_REFERENCED_BY_FRAGMENT_SHADER = 0x930A;
    public const int GL_REFERENCED_BY_COMPUTE_SHADER = 0x930B;
    public const int GL_TOP_LEVEL_ARRAY_SIZE = 0x930C;
    public const int GL_TOP_LEVEL_ARRAY_STRIDE = 0x930D;
    public const int GL_LOCATION = 0x930E;
    public const int GL_LOCATION_INDEX = 0x930F;
    public const int GL_IS_PER_PATCH = 0x92E7;
    public const int GL_SHADER_STORAGE_BUFFER = 0x90D2;
    public const int GL_SHADER_STORAGE_BUFFER_BINDING = 0x90D3;
    public const int GL_SHADER_STORAGE_BUFFER_START = 0x90D4;
    public const int GL_SHADER_STORAGE_BUFFER_SIZE = 0x90D5;
    public const int GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS = 0x90D6;
    public const int GL_MAX_GEOMETRY_SHADER_STORAGE_BLOCKS = 0x90D7;
    public const int GL_MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS = 0x90D8;
    public const int GL_MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS = 0x90D9;
    public const int GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS = 0x90DA;
    public const int GL_MAX_COMPUTE_SHADER_STORAGE_BLOCKS = 0x90DB;
    public const int GL_MAX_COMBINED_SHADER_STORAGE_BLOCKS = 0x90DC;
    public const int GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS = 0x90DD;
    public const int GL_MAX_SHADER_STORAGE_BLOCK_SIZE = 0x90DE;
    public const int GL_SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT = 0x90DF;
    public const int GL_SHADER_STORAGE_BARRIER_BIT = 0x00002000;
    public const int GL_MAX_COMBINED_SHADER_OUTPUT_RESOURCES = 0x8F39;
    public const int GL_DEPTH_STENCIL_TEXTURE_MODE = 0x90EA;
    public const int GL_TEXTURE_BUFFER_OFFSET = 0x919D;
    public const int GL_TEXTURE_BUFFER_SIZE = 0x919E;
    public const int GL_TEXTURE_BUFFER_OFFSET_ALIGNMENT = 0x919F;
    public const int GL_TEXTURE_VIEW_MIN_LEVEL = 0x82DB;
    public const int GL_TEXTURE_VIEW_NUM_LEVELS = 0x82DC;
    public const int GL_TEXTURE_VIEW_MIN_LAYER = 0x82DD;
    public const int GL_TEXTURE_VIEW_NUM_LAYERS = 0x82DE;
    public const int GL_TEXTURE_IMMUTABLE_LEVELS = 0x82DF;
    public const int GL_VERTEX_ATTRIB_BINDING = 0x82D4;
    public const int GL_VERTEX_ATTRIB_RELATIVE_OFFSET = 0x82D5;
    public const int GL_VERTEX_BINDING_DIVISOR = 0x82D6;
    public const int GL_VERTEX_BINDING_OFFSET = 0x82D7;
    public const int GL_VERTEX_BINDING_STRIDE = 0x82D8;
    public const int GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET = 0x82D9;
    public const int GL_MAX_VERTEX_ATTRIB_BINDINGS = 0x82DA;
    public const int GL_VERTEX_BINDING_BUFFER = 0x8F4F;
    public const int GL_DISPLAY_LIST = 0x82E7;
    public const int GL_STACK_UNDERFLOW = 0x0504;
    public const int GL_STACK_OVERFLOW = 0x0503;
    public const int GL_MAX_VERTEX_ATTRIB_STRIDE = 0x82E5;
    public const int GL_PRIMITIVE_RESTART_FOR_PATCHES_SUPPORTED = 0x8221;
    public const int GL_TEXTURE_BUFFER_BINDING = 0x8C2A;
    public const int GL_MAP_PERSISTENT_BIT = 0x0040;
    public const int GL_MAP_COHERENT_BIT = 0x0080;
    public const int GL_DYNAMIC_STORAGE_BIT = 0x0100;
    public const int GL_CLIENT_STORAGE_BIT = 0x0200;
    public const int GL_CLIENT_MAPPED_BUFFER_BARRIER_BIT = 0x00004000;
    public const int GL_BUFFER_IMMUTABLE_STORAGE = 0x821F;
    public const int GL_BUFFER_STORAGE_FLAGS = 0x8220;
    public const int GL_CLEAR_TEXTURE = 0x9365;
    public const int GL_LOCATION_COMPONENT = 0x934A;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_INDEX = 0x934B;
    public const int GL_TRANSFORM_FEEDBACK_BUFFER_STRIDE = 0x934C;
    public const int GL_QUERY_BUFFER = 0x9192;
    public const int GL_QUERY_BUFFER_BARRIER_BIT = 0x00008000;
    public const int GL_QUERY_BUFFER_BINDING = 0x9193;
    public const int GL_QUERY_RESULT_NO_WAIT = 0x9194;
    public const int GL_MIRROR_CLAMP_TO_EDGE = 0x8743;
    public const int GL_CONTEXT_LOST = 0x0507;
    public const int GL_NEGATIVE_ONE_TO_ONE = 0x935E;
    public const int GL_ZERO_TO_ONE = 0x935F;
    public const int GL_CLIP_ORIGIN = 0x935C;
    public const int GL_CLIP_DEPTH_MODE = 0x935D;
    public const int GL_QUERY_WAIT_INVERTED = 0x8E17;
    public const int GL_QUERY_NO_WAIT_INVERTED = 0x8E18;
    public const int GL_QUERY_BY_REGION_WAIT_INVERTED = 0x8E19;
    public const int GL_QUERY_BY_REGION_NO_WAIT_INVERTED = 0x8E1A;
    public const int GL_MAX_CULL_DISTANCES = 0x82F9;
    public const int GL_MAX_COMBINED_CLIP_AND_CULL_DISTANCES = 0x82FA;
    public const int GL_TEXTURE_TARGET = 0x1006;
    public const int GL_QUERY_TARGET = 0x82EA;
    public const int GL_GUILTY_CONTEXT_RESET = 0x8253;
    public const int GL_INNOCENT_CONTEXT_RESET = 0x8254;
    public const int GL_UNKNOWN_CONTEXT_RESET = 0x8255;
    public const int GL_RESET_NOTIFICATION_STRATEGY = 0x8256;
    public const int GL_LOSE_CONTEXT_ON_RESET = 0x8252;
    public const int GL_NO_RESET_NOTIFICATION = 0x8261;
    public const int GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT = 0x00000004;
    public const int GL_COLOR_TABLE = 0x80D0;
    public const int GL_POST_CONVOLUTION_COLOR_TABLE = 0x80D1;
    public const int GL_POST_COLOR_MATRIX_COLOR_TABLE = 0x80D2;
    public const int GL_PROXY_COLOR_TABLE = 0x80D3;
    public const int GL_PROXY_POST_CONVOLUTION_COLOR_TABLE = 0x80D4;
    public const int GL_PROXY_POST_COLOR_MATRIX_COLOR_TABLE = 0x80D5;
    public const int GL_CONVOLUTION_1D = 0x8010;
    public const int GL_CONVOLUTION_2D = 0x8011;
    public const int GL_SEPARABLE_2D = 0x8012;
    public const int GL_HISTOGRAM = 0x8024;
    public const int GL_PROXY_HISTOGRAM = 0x8025;
    public const int GL_MINMAX = 0x802E;
    public const int GL_CONTEXT_RELEASE_BEHAVIOR = 0x82FB;
    public const int GL_CONTEXT_RELEASE_BEHAVIOR_FLUSH = 0x82FC;
    public const int GL_SHADER_BINARY_FORMAT_SPIR_V = 0x9551;
    public const int GL_SPIR_V_BINARY = 0x9552;
    public const int GL_PARAMETER_BUFFER = 0x80EE;
    public const int GL_PARAMETER_BUFFER_BINDING = 0x80EF;
    public const int GL_CONTEXT_FLAG_NO_ERROR_BIT = 0x00000008;
    public const int GL_VERTICES_SUBMITTED = 0x82EE;
    public const int GL_PRIMITIVES_SUBMITTED = 0x82EF;
    public const int GL_VERTEX_SHADER_INVOCATIONS = 0x82F0;
    public const int GL_TESS_CONTROL_SHADER_PATCHES = 0x82F1;
    public const int GL_TESS_EVALUATION_SHADER_INVOCATIONS = 0x82F2;
    public const int GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED = 0x82F3;
    public const int GL_FRAGMENT_SHADER_INVOCATIONS = 0x82F4;
    public const int GL_COMPUTE_SHADER_INVOCATIONS = 0x82F5;
    public const int GL_CLIPPING_INPUT_PRIMITIVES = 0x82F6;
    public const int GL_CLIPPING_OUTPUT_PRIMITIVES = 0x82F7;
    public const int GL_POLYGON_OFFSET_CLAMP = 0x8E1B;
    public const int GL_SPIR_V_EXTENSIONS = 0x9553;
    public const int GL_NUM_SPIR_V_EXTENSIONS = 0x9554;
    public const int GL_TEXTURE_MAX_ANISOTROPY = 0x84FE;
    public const int GL_MAX_TEXTURE_MAX_ANISOTROPY = 0x84FF;
    public const int GL_TRANSFORM_FEEDBACK_OVERFLOW = 0x82EC;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GLDEBUGPROC(uint source, uint type, uint id, uint severity, int length, string message, IntPtr userParam);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLGETSTRINGPROC(uint name);
    private static PFNGLGETSTRINGPROC _glGetString;
    public static string glGetString(uint name) => Marshal.PtrToStringAnsi(_glGetString(name));

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERSOURCEPROC(uint shader, int count, string[] @string, in int length);
    private static PFNGLSHADERSOURCEPROC _glShaderSource;
    public static void glShaderSource(uint shader, int count, string[] @string, in int length) =>
        _glShaderSource(shader, count, @string, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMINFOLOGPROC(uint program, int bufSize, out int length, byte[] infoLog);
    private static PFNGLGETPROGRAMINFOLOGPROC _glGetProgramInfoLog;
    public static void glGetProgramInfoLog(uint program, int bufSize, out int length, out string infoLog)
    {
        var data = new byte[bufSize];
        _glGetProgramInfoLog(program, bufSize, out length, data);
        infoLog = Encoding.ASCII.GetString(data);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERINFOLOGPROC(uint shader, int bufSize, out int length, byte[] infoLog);
    private static PFNGLGETSHADERINFOLOGPROC _glGetShaderInfoLog;
    public static void glGetShaderInfoLog(uint shader, int bufSize, out int length, out string infoLog)
    {
        var data = new byte[bufSize];
        _glGetShaderInfoLog(shader, bufSize, out length, data);
        infoLog = Encoding.ASCII.GetString(data);
    }

    public static void glBufferData(uint target, ulong size, float[] data, uint usage)
    {
        fixed(void* ptr = data)
            _glBufferData(target, size, (IntPtr)ptr, usage);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCULLFACEPROC(uint mode);
    private static PFNGLCULLFACEPROC _glCullFace;
    public static void glCullFace(uint mode) => _glCullFace(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRONTFACEPROC(uint mode);
    private static PFNGLFRONTFACEPROC _glFrontFace;
    public static void glFrontFace(uint mode) => _glFrontFace(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLHINTPROC(uint target, uint mode);
    private static PFNGLHINTPROC _glHint;
    public static void glHint(uint target, uint mode) => _glHint(target, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLLINEWIDTHPROC(float width);
    private static PFNGLLINEWIDTHPROC _glLineWidth;
    public static void glLineWidth(float width) => _glLineWidth(width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTSIZEPROC(float size);
    private static PFNGLPOINTSIZEPROC _glPointSize;
    public static void glPointSize(float size) => _glPointSize(size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOLYGONMODEPROC(uint face, uint mode);
    private static PFNGLPOLYGONMODEPROC _glPolygonMode;
    public static void glPolygonMode(uint face, uint mode) => _glPolygonMode(face, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORPROC(int x, int y, int width, int height);
    private static PFNGLSCISSORPROC _glScissor;
    public static void glScissor(int x, int y, int width, int height) => _glScissor(x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERFPROC(uint target, uint pname, float param);
    private static PFNGLTEXPARAMETERFPROC _glTexParameterf;
    public static void glTexParameterf(uint target, uint pname, float param) => _glTexParameterf(target, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERFVPROC(uint target, uint pname, in float @params);
    private static PFNGLTEXPARAMETERFVPROC _glTexParameterfv;
    public static void glTexParameterfv(uint target, uint pname, in float @params) => _glTexParameterfv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIPROC(uint target, uint pname, int param);
    private static PFNGLTEXPARAMETERIPROC _glTexParameteri;
    public static void glTexParameteri(uint target, uint pname, int param) => _glTexParameteri(target, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIVPROC(uint target, uint pname, in int @params);
    private static PFNGLTEXPARAMETERIVPROC _glTexParameteriv;
    public static void glTexParameteriv(uint target, uint pname, in int @params) => _glTexParameteriv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE1DPROC(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXIMAGE1DPROC _glTexImage1D;
    public static void glTexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels) => _glTexImage1D(target, level, internalformat, width, border, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE2DPROC(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXIMAGE2DPROC _glTexImage2D;
    public static void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels) => _glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWBUFFERPROC(uint buf);
    private static PFNGLDRAWBUFFERPROC _glDrawBuffer;
    public static void glDrawBuffer(uint buf) => _glDrawBuffer(buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARPROC(uint mask);
    private static PFNGLCLEARPROC _glClear;
    public static void glClear(uint mask) => _glClear(mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARCOLORPROC(float red, float green, float blue, float alpha);
    private static PFNGLCLEARCOLORPROC _glClearColor;
    public static void glClearColor(float red, float green, float blue, float alpha) => _glClearColor(red, green, blue, alpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARSTENCILPROC(int s);
    private static PFNGLCLEARSTENCILPROC _glClearStencil;
    public static void glClearStencil(int s) => _glClearStencil(s);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARDEPTHPROC(double depth);
    private static PFNGLCLEARDEPTHPROC _glClearDepth;
    public static void glClearDepth(double depth) => _glClearDepth(depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILMASKPROC(uint mask);
    private static PFNGLSTENCILMASKPROC _glStencilMask;
    public static void glStencilMask(uint mask) => _glStencilMask(mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORMASKPROC(bool red, bool green, bool blue, bool alpha);
    private static PFNGLCOLORMASKPROC _glColorMask;
    public static void glColorMask(bool red, bool green, bool blue, bool alpha) => _glColorMask(red, green, blue, alpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHMASKPROC(bool flag);
    private static PFNGLDEPTHMASKPROC _glDepthMask;
    public static void glDepthMask(bool flag) => _glDepthMask(flag);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEPROC(uint cap);
    private static PFNGLDISABLEPROC _glDisable;
    public static void glDisable(uint cap) => _glDisable(cap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEPROC(uint cap);
    private static PFNGLENABLEPROC _glEnable;
    public static void glEnable(uint cap) => _glEnable(cap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFINISHPROC();
    private static PFNGLFINISHPROC _glFinish;
    public static void glFinish() => _glFinish();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFLUSHPROC();
    private static PFNGLFLUSHPROC _glFlush;
    public static void glFlush() => _glFlush();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCPROC(uint sfactor, uint dfactor);
    private static PFNGLBLENDFUNCPROC _glBlendFunc;
    public static void glBlendFunc(uint sfactor, uint dfactor) => _glBlendFunc(sfactor, dfactor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLLOGICOPPROC(uint opcode);
    private static PFNGLLOGICOPPROC _glLogicOp;
    public static void glLogicOp(uint opcode) => _glLogicOp(opcode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILFUNCPROC(uint func, int @ref, uint mask);
    private static PFNGLSTENCILFUNCPROC _glStencilFunc;
    public static void glStencilFunc(uint func, int @ref, uint mask) => _glStencilFunc(func, @ref, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILOPPROC(uint fail, uint zfail, uint zpass);
    private static PFNGLSTENCILOPPROC _glStencilOp;
    public static void glStencilOp(uint fail, uint zfail, uint zpass) => _glStencilOp(fail, zfail, zpass);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHFUNCPROC(uint func);
    private static PFNGLDEPTHFUNCPROC _glDepthFunc;
    public static void glDepthFunc(uint func) => _glDepthFunc(func);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPIXELSTOREFPROC(uint pname, float param);
    private static PFNGLPIXELSTOREFPROC _glPixelStoref;
    public static void glPixelStoref(uint pname, float param) => _glPixelStoref(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPIXELSTOREIPROC(uint pname, int param);
    private static PFNGLPIXELSTOREIPROC _glPixelStorei;
    public static void glPixelStorei(uint pname, int param) => _glPixelStorei(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLREADBUFFERPROC(uint src);
    private static PFNGLREADBUFFERPROC _glReadBuffer;
    public static void glReadBuffer(uint src) => _glReadBuffer(src);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLREADPIXELSPROC(int x, int y, int width, int height, uint format, uint type, IntPtr pixels);
    private static PFNGLREADPIXELSPROC _glReadPixels;
    public static void glReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pixels) => _glReadPixels(x, y, width, height, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBOOLEANVPROC(uint pname, out bool data);
    private static PFNGLGETBOOLEANVPROC _glGetBooleanv;
    public static void glGetBooleanv(uint pname, out bool data) => _glGetBooleanv(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETDOUBLEVPROC(uint pname, out double data);
    private static PFNGLGETDOUBLEVPROC _glGetDoublev;
    public static void glGetDoublev(uint pname, out double data) => _glGetDoublev(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETERRORPROC();
    private static PFNGLGETERRORPROC _glGetError;
    public static uint glGetError() => _glGetError();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFLOATVPROC(uint pname, out float data);
    private static PFNGLGETFLOATVPROC _glGetFloatv;
    public static void glGetFloatv(uint pname, out float data) => _glGetFloatv(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGERVPROC(uint pname, out int data);
    private static PFNGLGETINTEGERVPROC _glGetIntegerv;
    public static void glGetIntegerv(uint pname, out int data) => _glGetIntegerv(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXIMAGEPROC(uint target, int level, uint format, uint type, IntPtr pixels);
    private static PFNGLGETTEXIMAGEPROC _glGetTexImage;
    public static void glGetTexImage(uint target, int level, uint format, uint type, IntPtr pixels) => _glGetTexImage(target, level, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERFVPROC(uint target, uint pname, out float @params);
    private static PFNGLGETTEXPARAMETERFVPROC _glGetTexParameterfv;
    public static void glGetTexParameterfv(uint target, uint pname, out float @params) => _glGetTexParameterfv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETTEXPARAMETERIVPROC _glGetTexParameteriv;
    public static void glGetTexParameteriv(uint target, uint pname, out int @params) => _glGetTexParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXLEVELPARAMETERFVPROC(uint target, int level, uint pname, out float @params);
    private static PFNGLGETTEXLEVELPARAMETERFVPROC _glGetTexLevelParameterfv;
    public static void glGetTexLevelParameterfv(uint target, int level, uint pname, out float @params) => _glGetTexLevelParameterfv(target, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXLEVELPARAMETERIVPROC(uint target, int level, uint pname, out int @params);
    private static PFNGLGETTEXLEVELPARAMETERIVPROC _glGetTexLevelParameteriv;
    public static void glGetTexLevelParameteriv(uint target, int level, uint pname, out int @params) => _glGetTexLevelParameteriv(target, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISENABLEDPROC(uint cap);
    private static PFNGLISENABLEDPROC _glIsEnabled;
    public static bool glIsEnabled(uint cap) => _glIsEnabled(cap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEPROC(double n, double f);
    private static PFNGLDEPTHRANGEPROC _glDepthRange;
    public static void glDepthRange(double n, double f) => _glDepthRange(n, f);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTPROC(int x, int y, int width, int height);
    private static PFNGLVIEWPORTPROC _glViewport;
    public static void glViewport(int x, int y, int width, int height) => _glViewport(x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSPROC(uint mode, int first, int count);
    private static PFNGLDRAWARRAYSPROC _glDrawArrays;
    public static void glDrawArrays(uint mode, int first, int count) => _glDrawArrays(mode, first, count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSPROC(uint mode, int count, uint type, IntPtr indices);
    private static PFNGLDRAWELEMENTSPROC _glDrawElements;
    public static void glDrawElements(uint mode, int count, uint type, IntPtr indices) => _glDrawElements(mode, count, type, indices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOLYGONOFFSETPROC(float factor, float units);
    private static PFNGLPOLYGONOFFSETPROC _glPolygonOffset;
    public static void glPolygonOffset(float factor, float units) => _glPolygonOffset(factor, units);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXIMAGE1DPROC(uint target, int level, uint internalformat, int x, int y, int width, int border);
    private static PFNGLCOPYTEXIMAGE1DPROC _glCopyTexImage1D;
    public static void glCopyTexImage1D(uint target, int level, uint internalformat, int x, int y, int width, int border) => _glCopyTexImage1D(target, level, internalformat, x, y, width, border);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXIMAGE2DPROC(uint target, int level, uint internalformat, int x, int y, int width, int height, int border);
    private static PFNGLCOPYTEXIMAGE2DPROC _glCopyTexImage2D;
    public static void glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border) => _glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXSUBIMAGE1DPROC(uint target, int level, int xoffset, int x, int y, int width);
    private static PFNGLCOPYTEXSUBIMAGE1DPROC _glCopyTexSubImage1D;
    public static void glCopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width) => _glCopyTexSubImage1D(target, level, xoffset, x, y, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXSUBIMAGE2DPROC(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXSUBIMAGE2DPROC _glCopyTexSubImage2D;
    public static void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => _glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSUBIMAGE1DPROC(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXSUBIMAGE1DPROC _glTexSubImage1D;
    public static void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels) => _glTexSubImage1D(target, level, xoffset, width, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSUBIMAGE2DPROC(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXSUBIMAGE2DPROC _glTexSubImage2D;
    public static void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels) => _glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTUREPROC(uint target, uint texture);
    private static PFNGLBINDTEXTUREPROC _glBindTexture;
    public static void glBindTexture(uint target, uint texture) => _glBindTexture(target, texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETETEXTURESPROC(int n, in uint textures);
    private static PFNGLDELETETEXTURESPROC _glDeleteTextures;
    public static void glDeleteTextures(int n, in uint textures) => _glDeleteTextures(n, in textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENTEXTURESPROC(int n, out uint textures);
    private static PFNGLGENTEXTURESPROC _glGenTextures;
    public static void glGenTextures(int n, out uint textures) => _glGenTextures(n, out textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISTEXTUREPROC(uint texture);
    private static PFNGLISTEXTUREPROC _glIsTexture;
    public static bool glIsTexture(uint texture) => _glIsTexture(texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWRANGEELEMENTSPROC(uint mode, uint start, uint end, int count, uint type, IntPtr indices);
    private static PFNGLDRAWRANGEELEMENTSPROC _glDrawRangeElements;
    public static void glDrawRangeElements(uint mode, uint start, uint end, int count, uint type, IntPtr indices) => _glDrawRangeElements(mode, start, end, count, type, indices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE3DPROC(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXIMAGE3DPROC _glTexImage3D;
    public static void glTexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels) => _glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSUBIMAGE3DPROC(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXSUBIMAGE3DPROC _glTexSubImage3D;
    public static void glTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels) => _glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXSUBIMAGE3DPROC(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXSUBIMAGE3DPROC _glCopyTexSubImage3D;
    public static void glCopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => _glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLACTIVETEXTUREPROC(uint texture);
    private static PFNGLACTIVETEXTUREPROC _glActiveTexture;
    public static void glActiveTexture(uint texture) => _glActiveTexture(texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLECOVERAGEPROC(float value, bool invert);
    private static PFNGLSAMPLECOVERAGEPROC _glSampleCoverage;
    public static void glSampleCoverage(float value, bool invert) => _glSampleCoverage(value, invert);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE3DPROC(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXIMAGE3DPROC _glCompressedTexImage3D;
    public static void glCompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data) => _glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE2DPROC(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXIMAGE2DPROC _glCompressedTexImage2D;
    public static void glCompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data) => _glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXIMAGE1DPROC(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXIMAGE1DPROC _glCompressedTexImage1D;
    public static void glCompressedTexImage1D(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data) => _glCompressedTexImage1D(target, level, internalformat, width, border, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC _glCompressedTexSubImage3D;
    public static void glCompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data) => _glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC _glCompressedTexSubImage2D;
    public static void glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data) => _glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC _glCompressedTexSubImage1D;
    public static void glCompressedTexSubImage1D(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data) => _glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXIMAGEPROC(uint target, int level, IntPtr img);
    private static PFNGLGETCOMPRESSEDTEXIMAGEPROC _glGetCompressedTexImage;
    public static void glGetCompressedTexImage(uint target, int level, IntPtr img) => _glGetCompressedTexImage(target, level, img);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCSEPARATEPROC(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
    private static PFNGLBLENDFUNCSEPARATEPROC _glBlendFuncSeparate;
    public static void glBlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha) => _glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSPROC(uint mode, in int first, in int count, int drawcount);
    private static PFNGLMULTIDRAWARRAYSPROC _glMultiDrawArrays;
    public static void glMultiDrawArrays(uint mode, in int first, in int count, int drawcount) => _glMultiDrawArrays(mode, in first, in count, drawcount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSPROC(uint mode, in int count, uint type, IntPtr indices, int drawcount);
    private static PFNGLMULTIDRAWELEMENTSPROC _glMultiDrawElements;
    public static void glMultiDrawElements(uint mode, in int count, uint type, IntPtr indices, int drawcount) => _glMultiDrawElements(mode, in count, type, indices, drawcount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERFPROC(uint pname, float param);
    private static PFNGLPOINTPARAMETERFPROC _glPointParameterf;
    public static void glPointParameterf(uint pname, float param) => _glPointParameterf(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERFVPROC(uint pname, in float @params);
    private static PFNGLPOINTPARAMETERFVPROC _glPointParameterfv;
    public static void glPointParameterfv(uint pname, in float @params) => _glPointParameterfv(pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERIPROC(uint pname, int param);
    private static PFNGLPOINTPARAMETERIPROC _glPointParameteri;
    public static void glPointParameteri(uint pname, int param) => _glPointParameteri(pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOINTPARAMETERIVPROC(uint pname, in int @params);
    private static PFNGLPOINTPARAMETERIVPROC _glPointParameteriv;
    public static void glPointParameteriv(uint pname, in int @params) => _glPointParameteriv(pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDCOLORPROC(float red, float green, float blue, float alpha);
    private static PFNGLBLENDCOLORPROC _glBlendColor;
    public static void glBlendColor(float red, float green, float blue, float alpha) => _glBlendColor(red, green, blue, alpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONPROC(uint mode);
    private static PFNGLBLENDEQUATIONPROC _glBlendEquation;
    public static void glBlendEquation(uint mode) => _glBlendEquation(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENQUERIESPROC(int n, out uint ids);
    private static PFNGLGENQUERIESPROC _glGenQueries;
    public static void glGenQueries(int n, out uint ids) => _glGenQueries(n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEQUERIESPROC(int n, in uint ids);
    private static PFNGLDELETEQUERIESPROC _glDeleteQueries;
    public static void glDeleteQueries(int n, in uint ids) => _glDeleteQueries(n, in ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISQUERYPROC(uint id);
    private static PFNGLISQUERYPROC _glIsQuery;
    public static bool glIsQuery(uint id) => _glIsQuery(id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINQUERYPROC(uint target, uint id);
    private static PFNGLBEGINQUERYPROC _glBeginQuery;
    public static void glBeginQuery(uint target, uint id) => _glBeginQuery(target, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDQUERYPROC(uint target);
    private static PFNGLENDQUERYPROC _glEndQuery;
    public static void glEndQuery(uint target) => _glEndQuery(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETQUERYIVPROC _glGetQueryiv;
    public static void glGetQueryiv(uint target, uint pname, out int @params) => _glGetQueryiv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTIVPROC(uint id, uint pname, out int @params);
    private static PFNGLGETQUERYOBJECTIVPROC _glGetQueryObjectiv;
    public static void glGetQueryObjectiv(uint id, uint pname, out int @params) => _glGetQueryObjectiv(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTUIVPROC(uint id, uint pname, out uint @params);
    private static PFNGLGETQUERYOBJECTUIVPROC _glGetQueryObjectuiv;
    public static void glGetQueryObjectuiv(uint id, uint pname, out uint @params) => _glGetQueryObjectuiv(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERPROC(uint target, uint buffer);
    private static PFNGLBINDBUFFERPROC _glBindBuffer;
    public static void glBindBuffer(uint target, uint buffer) => _glBindBuffer(target, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEBUFFERSPROC(int n, in uint buffers);
    private static PFNGLDELETEBUFFERSPROC _glDeleteBuffers;
    public static void glDeleteBuffers(int n, in uint buffers) => _glDeleteBuffers(n, in buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENBUFFERSPROC(int n, out uint buffers);
    private static PFNGLGENBUFFERSPROC _glGenBuffers;
    public static void glGenBuffers(int n, out uint buffers) => _glGenBuffers(n, out buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISBUFFERPROC(uint buffer);
    private static PFNGLISBUFFERPROC _glIsBuffer;
    public static bool glIsBuffer(uint buffer) => _glIsBuffer(buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBUFFERDATAPROC(uint target, ulong size, IntPtr data, uint usage);
    private static PFNGLBUFFERDATAPROC _glBufferData;
    public static void glBufferData(uint target, ulong size, IntPtr data, uint usage) => _glBufferData(target, size, data, usage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBUFFERSUBDATAPROC(uint target, ulong offset, ulong size, IntPtr data);
    private static PFNGLBUFFERSUBDATAPROC _glBufferSubData;
    public static void glBufferSubData(uint target, ulong offset, ulong size, IntPtr data) => _glBufferSubData(target, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERSUBDATAPROC(uint target, ulong offset, ulong size, IntPtr data);
    private static PFNGLGETBUFFERSUBDATAPROC _glGetBufferSubData;
    public static void glGetBufferSubData(uint target, ulong offset, ulong size, IntPtr data) => _glGetBufferSubData(target, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPBUFFERPROC(uint target, uint access);
    private static PFNGLMAPBUFFERPROC _glMapBuffer;
    public static IntPtr glMapBuffer(uint target, uint access) => _glMapBuffer(target, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLUNMAPBUFFERPROC(uint target);
    private static PFNGLUNMAPBUFFERPROC _glUnmapBuffer;
    public static bool glUnmapBuffer(uint target) => _glUnmapBuffer(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETBUFFERPARAMETERIVPROC _glGetBufferParameteriv;
    public static void glGetBufferParameteriv(uint target, uint pname, out int @params) => _glGetBufferParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERPOINTERVPROC(uint target, uint pname, IntPtr @params);
    private static PFNGLGETBUFFERPOINTERVPROC _glGetBufferPointerv;
    public static void glGetBufferPointerv(uint target, uint pname, IntPtr @params) => _glGetBufferPointerv(target, pname, @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONSEPARATEPROC(uint modeRGB, uint modeAlpha);
    private static PFNGLBLENDEQUATIONSEPARATEPROC _glBlendEquationSeparate;
    public static void glBlendEquationSeparate(uint modeRGB, uint modeAlpha) => _glBlendEquationSeparate(modeRGB, modeAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWBUFFERSPROC(int n, in uint bufs);
    private static PFNGLDRAWBUFFERSPROC _glDrawBuffers;
    public static void glDrawBuffers(int n, in uint bufs) => _glDrawBuffers(n, in bufs);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILOPSEPARATEPROC(uint face, uint sfail, uint dpfail, uint dppass);
    private static PFNGLSTENCILOPSEPARATEPROC _glStencilOpSeparate;
    public static void glStencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass) => _glStencilOpSeparate(face, sfail, dpfail, dppass);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILFUNCSEPARATEPROC(uint face, uint func, int @ref, uint mask);
    private static PFNGLSTENCILFUNCSEPARATEPROC _glStencilFuncSeparate;
    public static void glStencilFuncSeparate(uint face, uint func, int @ref, uint mask) => _glStencilFuncSeparate(face, func, @ref, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSTENCILMASKSEPARATEPROC(uint face, uint mask);
    private static PFNGLSTENCILMASKSEPARATEPROC _glStencilMaskSeparate;
    public static void glStencilMaskSeparate(uint face, uint mask) => _glStencilMaskSeparate(face, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLATTACHSHADERPROC(uint program, uint shader);
    private static PFNGLATTACHSHADERPROC _glAttachShader;
    public static void glAttachShader(uint program, uint shader) => _glAttachShader(program, shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDATTRIBLOCATIONPROC(uint program, uint index, string name);
    private static PFNGLBINDATTRIBLOCATIONPROC _glBindAttribLocation;
    public static void glBindAttribLocation(uint program, uint index, string name) => _glBindAttribLocation(program, index, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPILESHADERPROC(uint shader);
    private static PFNGLCOMPILESHADERPROC _glCompileShader;
    public static void glCompileShader(uint shader) => _glCompileShader(shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCREATEPROGRAMPROC();
    private static PFNGLCREATEPROGRAMPROC _glCreateProgram;
    public static uint glCreateProgram() => _glCreateProgram();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCREATESHADERPROC(uint type);
    private static PFNGLCREATESHADERPROC _glCreateShader;
    public static uint glCreateShader(uint type) => _glCreateShader(type);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEPROGRAMPROC(uint program);
    private static PFNGLDELETEPROGRAMPROC _glDeleteProgram;
    public static void glDeleteProgram(uint program) => _glDeleteProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETESHADERPROC(uint shader);
    private static PFNGLDELETESHADERPROC _glDeleteShader;
    public static void glDeleteShader(uint shader) => _glDeleteShader(shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDETACHSHADERPROC(uint program, uint shader);
    private static PFNGLDETACHSHADERPROC _glDetachShader;
    public static void glDetachShader(uint program, uint shader) => _glDetachShader(program, shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEVERTEXATTRIBARRAYPROC(uint index);
    private static PFNGLDISABLEVERTEXATTRIBARRAYPROC _glDisableVertexAttribArray;
    public static void glDisableVertexAttribArray(uint index) => _glDisableVertexAttribArray(index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEVERTEXATTRIBARRAYPROC(uint index);
    private static PFNGLENABLEVERTEXATTRIBARRAYPROC _glEnableVertexAttribArray;
    public static void glEnableVertexAttribArray(uint index) => _glEnableVertexAttribArray(index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEATTRIBPROC(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name);
    private static PFNGLGETACTIVEATTRIBPROC _glGetActiveAttrib;
    public static void glGetActiveAttrib(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name) => _glGetActiveAttrib(program, index, bufSize, out length, out size, out type, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMPROC(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name);
    private static PFNGLGETACTIVEUNIFORMPROC _glGetActiveUniform;
    public static void glGetActiveUniform(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name) => _glGetActiveUniform(program, index, bufSize, out length, out size, out type, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETATTACHEDSHADERSPROC(uint program, int maxCount, out int count, out uint shaders);
    private static PFNGLGETATTACHEDSHADERSPROC _glGetAttachedShaders;
    public static void glGetAttachedShaders(uint program, int maxCount, out int count, out uint shaders) => _glGetAttachedShaders(program, maxCount, out count, out shaders);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETATTRIBLOCATIONPROC(uint program, string name);
    private static PFNGLGETATTRIBLOCATIONPROC _glGetAttribLocation;
    public static int glGetAttribLocation(uint program, string name) => _glGetAttribLocation(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMIVPROC(uint program, uint pname, out int @params);
    private static PFNGLGETPROGRAMIVPROC _glGetProgramiv;
    public static void glGetProgramiv(uint program, uint pname, out int @params) => _glGetProgramiv(program, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERIVPROC(uint shader, uint pname, out int @params);
    private static PFNGLGETSHADERIVPROC _glGetShaderiv;
    public static void glGetShaderiv(uint shader, uint pname, out int @params) => _glGetShaderiv(shader, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERSOURCEPROC(uint shader, int bufSize, out int length, string source);
    private static PFNGLGETSHADERSOURCEPROC _glGetShaderSource;
    public static void glGetShaderSource(uint shader, int bufSize, out int length, string source) => _glGetShaderSource(shader, bufSize, out length, source);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETUNIFORMLOCATIONPROC(uint program, string name);
    private static PFNGLGETUNIFORMLOCATIONPROC _glGetUniformLocation;
    public static int glGetUniformLocation(uint program, string name) => _glGetUniformLocation(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMFVPROC(uint program, int location, out float @params);
    private static PFNGLGETUNIFORMFVPROC _glGetUniformfv;
    public static void glGetUniformfv(uint program, int location, out float @params) => _glGetUniformfv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMIVPROC(uint program, int location, out int @params);
    private static PFNGLGETUNIFORMIVPROC _glGetUniformiv;
    public static void glGetUniformiv(uint program, int location, out int @params) => _glGetUniformiv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBDVPROC(uint index, uint pname, out double @params);
    private static PFNGLGETVERTEXATTRIBDVPROC _glGetVertexAttribdv;
    public static void glGetVertexAttribdv(uint index, uint pname, out double @params) => _glGetVertexAttribdv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBFVPROC(uint index, uint pname, out float @params);
    private static PFNGLGETVERTEXATTRIBFVPROC _glGetVertexAttribfv;
    public static void glGetVertexAttribfv(uint index, uint pname, out float @params) => _glGetVertexAttribfv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBIVPROC(uint index, uint pname, out int @params);
    private static PFNGLGETVERTEXATTRIBIVPROC _glGetVertexAttribiv;
    public static void glGetVertexAttribiv(uint index, uint pname, out int @params) => _glGetVertexAttribiv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBPOINTERVPROC(uint index, uint pname, IntPtr pointer);
    private static PFNGLGETVERTEXATTRIBPOINTERVPROC _glGetVertexAttribPointerv;
    public static void glGetVertexAttribPointerv(uint index, uint pname, IntPtr pointer) => _glGetVertexAttribPointerv(index, pname, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISPROGRAMPROC(uint program);
    private static PFNGLISPROGRAMPROC _glIsProgram;
    public static bool glIsProgram(uint program) => _glIsProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISSHADERPROC(uint shader);
    private static PFNGLISSHADERPROC _glIsShader;
    public static bool glIsShader(uint shader) => _glIsShader(shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLLINKPROGRAMPROC(uint program);
    private static PFNGLLINKPROGRAMPROC _glLinkProgram;
    public static void glLinkProgram(uint program) => _glLinkProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUSEPROGRAMPROC(uint program);
    private static PFNGLUSEPROGRAMPROC _glUseProgram;
    public static void glUseProgram(uint program) => _glUseProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1FPROC(int location, float v0);
    private static PFNGLUNIFORM1FPROC _glUniform1f;
    public static void glUniform1f(int location, float v0) => _glUniform1f(location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2FPROC(int location, float v0, float v1);
    private static PFNGLUNIFORM2FPROC _glUniform2f;
    public static void glUniform2f(int location, float v0, float v1) => _glUniform2f(location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3FPROC(int location, float v0, float v1, float v2);
    private static PFNGLUNIFORM3FPROC _glUniform3f;
    public static void glUniform3f(int location, float v0, float v1, float v2) => _glUniform3f(location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4FPROC(int location, float v0, float v1, float v2, float v3);
    private static PFNGLUNIFORM4FPROC _glUniform4f;
    public static void glUniform4f(int location, float v0, float v1, float v2, float v3) => _glUniform4f(location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1IPROC(int location, int v0);
    private static PFNGLUNIFORM1IPROC _glUniform1i;
    public static void glUniform1i(int location, int v0) => _glUniform1i(location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2IPROC(int location, int v0, int v1);
    private static PFNGLUNIFORM2IPROC _glUniform2i;
    public static void glUniform2i(int location, int v0, int v1) => _glUniform2i(location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3IPROC(int location, int v0, int v1, int v2);
    private static PFNGLUNIFORM3IPROC _glUniform3i;
    public static void glUniform3i(int location, int v0, int v1, int v2) => _glUniform3i(location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4IPROC(int location, int v0, int v1, int v2, int v3);
    private static PFNGLUNIFORM4IPROC _glUniform4i;
    public static void glUniform4i(int location, int v0, int v1, int v2, int v3) => _glUniform4i(location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1FVPROC(int location, int count, in float value);
    private static PFNGLUNIFORM1FVPROC _glUniform1fv;
    public static void glUniform1fv(int location, int count, in float value) => _glUniform1fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2FVPROC(int location, int count, in float value);
    private static PFNGLUNIFORM2FVPROC _glUniform2fv;
    public static void glUniform2fv(int location, int count, in float value) => _glUniform2fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3FVPROC(int location, int count, in float value);
    private static PFNGLUNIFORM3FVPROC _glUniform3fv;
    public static void glUniform3fv(int location, int count, in float value) => _glUniform3fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4FVPROC(int location, int count, in float value);
    private static PFNGLUNIFORM4FVPROC _glUniform4fv;
    public static void glUniform4fv(int location, int count, in float value) => _glUniform4fv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1IVPROC(int location, int count, in int value);
    private static PFNGLUNIFORM1IVPROC _glUniform1iv;
    public static void glUniform1iv(int location, int count, in int value) => _glUniform1iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2IVPROC(int location, int count, in int value);
    private static PFNGLUNIFORM2IVPROC _glUniform2iv;
    public static void glUniform2iv(int location, int count, in int value) => _glUniform2iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3IVPROC(int location, int count, in int value);
    private static PFNGLUNIFORM3IVPROC _glUniform3iv;
    public static void glUniform3iv(int location, int count, in int value) => _glUniform3iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4IVPROC(int location, int count, in int value);
    private static PFNGLUNIFORM4IVPROC _glUniform4iv;
    public static void glUniform4iv(int location, int count, in int value) => _glUniform4iv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX2FVPROC _glUniformMatrix2fv;
    public static void glUniformMatrix2fv(int location, int count, bool transpose, in float value) => _glUniformMatrix2fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX3FVPROC _glUniformMatrix3fv;
    public static void glUniformMatrix3fv(int location, int count, bool transpose, in float value) => _glUniformMatrix3fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX4FVPROC _glUniformMatrix4fv;
    public static void glUniformMatrix4fv(int location, int count, bool transpose, in float value) => _glUniformMatrix4fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVALIDATEPROGRAMPROC(uint program);
    private static PFNGLVALIDATEPROGRAMPROC _glValidateProgram;
    public static void glValidateProgram(uint program) => _glValidateProgram(program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1DPROC(uint index, double x);
    private static PFNGLVERTEXATTRIB1DPROC _glVertexAttrib1d;
    public static void glVertexAttrib1d(uint index, double x) => _glVertexAttrib1d(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIB1DVPROC _glVertexAttrib1dv;
    public static void glVertexAttrib1dv(uint index, in double v) => _glVertexAttrib1dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1FPROC(uint index, float x);
    private static PFNGLVERTEXATTRIB1FPROC _glVertexAttrib1f;
    public static void glVertexAttrib1f(uint index, float x) => _glVertexAttrib1f(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1FVPROC(uint index, in float v);
    private static PFNGLVERTEXATTRIB1FVPROC _glVertexAttrib1fv;
    public static void glVertexAttrib1fv(uint index, in float v) => _glVertexAttrib1fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1SPROC(uint index, short x);
    private static PFNGLVERTEXATTRIB1SPROC _glVertexAttrib1s;
    public static void glVertexAttrib1s(uint index, short x) => _glVertexAttrib1s(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB1SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB1SVPROC _glVertexAttrib1sv;
    public static void glVertexAttrib1sv(uint index, in short v) => _glVertexAttrib1sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2DPROC(uint index, double x, double y);
    private static PFNGLVERTEXATTRIB2DPROC _glVertexAttrib2d;
    public static void glVertexAttrib2d(uint index, double x, double y) => _glVertexAttrib2d(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIB2DVPROC _glVertexAttrib2dv;
    public static void glVertexAttrib2dv(uint index, in double v) => _glVertexAttrib2dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2FPROC(uint index, float x, float y);
    private static PFNGLVERTEXATTRIB2FPROC _glVertexAttrib2f;
    public static void glVertexAttrib2f(uint index, float x, float y) => _glVertexAttrib2f(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2FVPROC(uint index, in float v);
    private static PFNGLVERTEXATTRIB2FVPROC _glVertexAttrib2fv;
    public static void glVertexAttrib2fv(uint index, in float v) => _glVertexAttrib2fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2SPROC(uint index, short x, short y);
    private static PFNGLVERTEXATTRIB2SPROC _glVertexAttrib2s;
    public static void glVertexAttrib2s(uint index, short x, short y) => _glVertexAttrib2s(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB2SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB2SVPROC _glVertexAttrib2sv;
    public static void glVertexAttrib2sv(uint index, in short v) => _glVertexAttrib2sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3DPROC(uint index, double x, double y, double z);
    private static PFNGLVERTEXATTRIB3DPROC _glVertexAttrib3d;
    public static void glVertexAttrib3d(uint index, double x, double y, double z) => _glVertexAttrib3d(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIB3DVPROC _glVertexAttrib3dv;
    public static void glVertexAttrib3dv(uint index, in double v) => _glVertexAttrib3dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3FPROC(uint index, float x, float y, float z);
    private static PFNGLVERTEXATTRIB3FPROC _glVertexAttrib3f;
    public static void glVertexAttrib3f(uint index, float x, float y, float z) => _glVertexAttrib3f(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3FVPROC(uint index, in float v);
    private static PFNGLVERTEXATTRIB3FVPROC _glVertexAttrib3fv;
    public static void glVertexAttrib3fv(uint index, in float v) => _glVertexAttrib3fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3SPROC(uint index, short x, short y, short z);
    private static PFNGLVERTEXATTRIB3SPROC _glVertexAttrib3s;
    public static void glVertexAttrib3s(uint index, short x, short y, short z) => _glVertexAttrib3s(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB3SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB3SVPROC _glVertexAttrib3sv;
    public static void glVertexAttrib3sv(uint index, in short v) => _glVertexAttrib3sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NBVPROC(uint index, in sbyte v);
    private static PFNGLVERTEXATTRIB4NBVPROC _glVertexAttrib4Nbv;
    public static void glVertexAttrib4Nbv(uint index, in sbyte v) => _glVertexAttrib4Nbv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NIVPROC(uint index, in int v);
    private static PFNGLVERTEXATTRIB4NIVPROC _glVertexAttrib4Niv;
    public static void glVertexAttrib4Niv(uint index, in int v) => _glVertexAttrib4Niv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NSVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB4NSVPROC _glVertexAttrib4Nsv;
    public static void glVertexAttrib4Nsv(uint index, in short v) => _glVertexAttrib4Nsv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUBPROC(uint index, byte x, byte y, byte z, byte w);
    private static PFNGLVERTEXATTRIB4NUBPROC _glVertexAttrib4Nub;
    public static void glVertexAttrib4Nub(uint index, byte x, byte y, byte z, byte w) => _glVertexAttrib4Nub(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUBVPROC(uint index, IntPtr v);
    private static PFNGLVERTEXATTRIB4NUBVPROC _glVertexAttrib4Nubv;
    public static void glVertexAttrib4Nubv(uint index, IntPtr v) => _glVertexAttrib4Nubv(index, v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUIVPROC(uint index, in uint v);
    private static PFNGLVERTEXATTRIB4NUIVPROC _glVertexAttrib4Nuiv;
    public static void glVertexAttrib4Nuiv(uint index, in uint v) => _glVertexAttrib4Nuiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4NUSVPROC(uint index, in ushort v);
    private static PFNGLVERTEXATTRIB4NUSVPROC _glVertexAttrib4Nusv;
    public static void glVertexAttrib4Nusv(uint index, in ushort v) => _glVertexAttrib4Nusv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4BVPROC(uint index, in sbyte v);
    private static PFNGLVERTEXATTRIB4BVPROC _glVertexAttrib4bv;
    public static void glVertexAttrib4bv(uint index, in sbyte v) => _glVertexAttrib4bv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4DPROC(uint index, double x, double y, double z, double w);
    private static PFNGLVERTEXATTRIB4DPROC _glVertexAttrib4d;
    public static void glVertexAttrib4d(uint index, double x, double y, double z, double w) => _glVertexAttrib4d(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIB4DVPROC _glVertexAttrib4dv;
    public static void glVertexAttrib4dv(uint index, in double v) => _glVertexAttrib4dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4FPROC(uint index, float x, float y, float z, float w);
    private static PFNGLVERTEXATTRIB4FPROC _glVertexAttrib4f;
    public static void glVertexAttrib4f(uint index, float x, float y, float z, float w) => _glVertexAttrib4f(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4FVPROC(uint index, in float v);
    private static PFNGLVERTEXATTRIB4FVPROC _glVertexAttrib4fv;
    public static void glVertexAttrib4fv(uint index, in float v) => _glVertexAttrib4fv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4IVPROC(uint index, in int v);
    private static PFNGLVERTEXATTRIB4IVPROC _glVertexAttrib4iv;
    public static void glVertexAttrib4iv(uint index, in int v) => _glVertexAttrib4iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4SPROC(uint index, short x, short y, short z, short w);
    private static PFNGLVERTEXATTRIB4SPROC _glVertexAttrib4s;
    public static void glVertexAttrib4s(uint index, short x, short y, short z, short w) => _glVertexAttrib4s(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIB4SVPROC _glVertexAttrib4sv;
    public static void glVertexAttrib4sv(uint index, in short v) => _glVertexAttrib4sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4UBVPROC(uint index, IntPtr v);
    private static PFNGLVERTEXATTRIB4UBVPROC _glVertexAttrib4ubv;
    public static void glVertexAttrib4ubv(uint index, IntPtr v) => _glVertexAttrib4ubv(index, v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4UIVPROC(uint index, in uint v);
    private static PFNGLVERTEXATTRIB4UIVPROC _glVertexAttrib4uiv;
    public static void glVertexAttrib4uiv(uint index, in uint v) => _glVertexAttrib4uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIB4USVPROC(uint index, in ushort v);
    private static PFNGLVERTEXATTRIB4USVPROC _glVertexAttrib4usv;
    public static void glVertexAttrib4usv(uint index, in ushort v) => _glVertexAttrib4usv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBPOINTERPROC(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);
    private static PFNGLVERTEXATTRIBPOINTERPROC _glVertexAttribPointer;
    public static void glVertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer) => _glVertexAttribPointer(index, size, type, normalized, stride, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X3FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX2X3FVPROC _glUniformMatrix2x3fv;
    public static void glUniformMatrix2x3fv(int location, int count, bool transpose, in float value) => _glUniformMatrix2x3fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X2FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX3X2FVPROC _glUniformMatrix3x2fv;
    public static void glUniformMatrix3x2fv(int location, int count, bool transpose, in float value) => _glUniformMatrix3x2fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X4FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX2X4FVPROC _glUniformMatrix2x4fv;
    public static void glUniformMatrix2x4fv(int location, int count, bool transpose, in float value) => _glUniformMatrix2x4fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X2FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX4X2FVPROC _glUniformMatrix4x2fv;
    public static void glUniformMatrix4x2fv(int location, int count, bool transpose, in float value) => _glUniformMatrix4x2fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X4FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX3X4FVPROC _glUniformMatrix3x4fv;
    public static void glUniformMatrix3x4fv(int location, int count, bool transpose, in float value) => _glUniformMatrix3x4fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X3FVPROC(int location, int count, bool transpose, in float value);
    private static PFNGLUNIFORMMATRIX4X3FVPROC _glUniformMatrix4x3fv;
    public static void glUniformMatrix4x3fv(int location, int count, bool transpose, in float value) => _glUniformMatrix4x3fv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORMASKIPROC(uint index, bool r, bool g, bool b, bool a);
    private static PFNGLCOLORMASKIPROC _glColorMaski;
    public static void glColorMaski(uint index, bool r, bool g, bool b, bool a) => _glColorMaski(index, r, g, b, a);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBOOLEANI_VPROC(uint target, uint index, out bool data);
    private static PFNGLGETBOOLEANI_VPROC _glGetBooleani_v;
    public static void glGetBooleani_v(uint target, uint index, out bool data) => _glGetBooleani_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGERI_VPROC(uint target, uint index, out int data);
    private static PFNGLGETINTEGERI_VPROC _glGetIntegeri_v;
    public static void glGetIntegeri_v(uint target, uint index, out int data) => _glGetIntegeri_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEIPROC(uint target, uint index);
    private static PFNGLENABLEIPROC _glEnablei;
    public static void glEnablei(uint target, uint index) => _glEnablei(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEIPROC(uint target, uint index);
    private static PFNGLDISABLEIPROC _glDisablei;
    public static void glDisablei(uint target, uint index) => _glDisablei(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISENABLEDIPROC(uint target, uint index);
    private static PFNGLISENABLEDIPROC _glIsEnabledi;
    public static bool glIsEnabledi(uint target, uint index) => _glIsEnabledi(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINTRANSFORMFEEDBACKPROC(uint primitiveMode);
    private static PFNGLBEGINTRANSFORMFEEDBACKPROC _glBeginTransformFeedback;
    public static void glBeginTransformFeedback(uint primitiveMode) => _glBeginTransformFeedback(primitiveMode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDTRANSFORMFEEDBACKPROC();
    private static PFNGLENDTRANSFORMFEEDBACKPROC _glEndTransformFeedback;
    public static void glEndTransformFeedback() => _glEndTransformFeedback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERRANGEPROC(uint target, uint index, uint buffer, ulong offset, ulong size);
    private static PFNGLBINDBUFFERRANGEPROC _glBindBufferRange;
    public static void glBindBufferRange(uint target, uint index, uint buffer, ulong offset, ulong size) => _glBindBufferRange(target, index, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERBASEPROC(uint target, uint index, uint buffer);
    private static PFNGLBINDBUFFERBASEPROC _glBindBufferBase;
    public static void glBindBufferBase(uint target, uint index, uint buffer) => _glBindBufferBase(target, index, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKVARYINGSPROC(uint program, int count, string varyings, uint bufferMode);
    private static PFNGLTRANSFORMFEEDBACKVARYINGSPROC _glTransformFeedbackVaryings;
    public static void glTransformFeedbackVaryings(uint program, int count, string varyings, uint bufferMode) => _glTransformFeedbackVaryings(program, count, varyings, bufferMode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKVARYINGPROC(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name);
    private static PFNGLGETTRANSFORMFEEDBACKVARYINGPROC _glGetTransformFeedbackVarying;
    public static void glGetTransformFeedbackVarying(uint program, uint index, int bufSize, out int length, out int size, out uint type, string name) => _glGetTransformFeedbackVarying(program, index, bufSize, out length, out size, out type, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLAMPCOLORPROC(uint target, uint clamp);
    private static PFNGLCLAMPCOLORPROC _glClampColor;
    public static void glClampColor(uint target, uint clamp) => _glClampColor(target, clamp);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINCONDITIONALRENDERPROC(uint id, uint mode);
    private static PFNGLBEGINCONDITIONALRENDERPROC _glBeginConditionalRender;
    public static void glBeginConditionalRender(uint id, uint mode) => _glBeginConditionalRender(id, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDCONDITIONALRENDERPROC();
    private static PFNGLENDCONDITIONALRENDERPROC _glEndConditionalRender;
    public static void glEndConditionalRender() => _glEndConditionalRender();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBIPOINTERPROC(uint index, int size, uint type, int stride, IntPtr pointer);
    private static PFNGLVERTEXATTRIBIPOINTERPROC _glVertexAttribIPointer;
    public static void glVertexAttribIPointer(uint index, int size, uint type, int stride, IntPtr pointer) => _glVertexAttribIPointer(index, size, type, stride, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBIIVPROC(uint index, uint pname, out int @params);
    private static PFNGLGETVERTEXATTRIBIIVPROC _glGetVertexAttribIiv;
    public static void glGetVertexAttribIiv(uint index, uint pname, out int @params) => _glGetVertexAttribIiv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBIUIVPROC(uint index, uint pname, out uint @params);
    private static PFNGLGETVERTEXATTRIBIUIVPROC _glGetVertexAttribIuiv;
    public static void glGetVertexAttribIuiv(uint index, uint pname, out uint @params) => _glGetVertexAttribIuiv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1IPROC(uint index, int x);
    private static PFNGLVERTEXATTRIBI1IPROC _glVertexAttribI1i;
    public static void glVertexAttribI1i(uint index, int x) => _glVertexAttribI1i(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2IPROC(uint index, int x, int y);
    private static PFNGLVERTEXATTRIBI2IPROC _glVertexAttribI2i;
    public static void glVertexAttribI2i(uint index, int x, int y) => _glVertexAttribI2i(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3IPROC(uint index, int x, int y, int z);
    private static PFNGLVERTEXATTRIBI3IPROC _glVertexAttribI3i;
    public static void glVertexAttribI3i(uint index, int x, int y, int z) => _glVertexAttribI3i(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4IPROC(uint index, int x, int y, int z, int w);
    private static PFNGLVERTEXATTRIBI4IPROC _glVertexAttribI4i;
    public static void glVertexAttribI4i(uint index, int x, int y, int z, int w) => _glVertexAttribI4i(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1UIPROC(uint index, uint x);
    private static PFNGLVERTEXATTRIBI1UIPROC _glVertexAttribI1ui;
    public static void glVertexAttribI1ui(uint index, uint x) => _glVertexAttribI1ui(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2UIPROC(uint index, uint x, uint y);
    private static PFNGLVERTEXATTRIBI2UIPROC _glVertexAttribI2ui;
    public static void glVertexAttribI2ui(uint index, uint x, uint y) => _glVertexAttribI2ui(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3UIPROC(uint index, uint x, uint y, uint z);
    private static PFNGLVERTEXATTRIBI3UIPROC _glVertexAttribI3ui;
    public static void glVertexAttribI3ui(uint index, uint x, uint y, uint z) => _glVertexAttribI3ui(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4UIPROC(uint index, uint x, uint y, uint z, uint w);
    private static PFNGLVERTEXATTRIBI4UIPROC _glVertexAttribI4ui;
    public static void glVertexAttribI4ui(uint index, uint x, uint y, uint z, uint w) => _glVertexAttribI4ui(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1IVPROC(uint index, in int v);
    private static PFNGLVERTEXATTRIBI1IVPROC _glVertexAttribI1iv;
    public static void glVertexAttribI1iv(uint index, in int v) => _glVertexAttribI1iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2IVPROC(uint index, in int v);
    private static PFNGLVERTEXATTRIBI2IVPROC _glVertexAttribI2iv;
    public static void glVertexAttribI2iv(uint index, in int v) => _glVertexAttribI2iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3IVPROC(uint index, in int v);
    private static PFNGLVERTEXATTRIBI3IVPROC _glVertexAttribI3iv;
    public static void glVertexAttribI3iv(uint index, in int v) => _glVertexAttribI3iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4IVPROC(uint index, in int v);
    private static PFNGLVERTEXATTRIBI4IVPROC _glVertexAttribI4iv;
    public static void glVertexAttribI4iv(uint index, in int v) => _glVertexAttribI4iv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI1UIVPROC(uint index, in uint v);
    private static PFNGLVERTEXATTRIBI1UIVPROC _glVertexAttribI1uiv;
    public static void glVertexAttribI1uiv(uint index, in uint v) => _glVertexAttribI1uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI2UIVPROC(uint index, in uint v);
    private static PFNGLVERTEXATTRIBI2UIVPROC _glVertexAttribI2uiv;
    public static void glVertexAttribI2uiv(uint index, in uint v) => _glVertexAttribI2uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI3UIVPROC(uint index, in uint v);
    private static PFNGLVERTEXATTRIBI3UIVPROC _glVertexAttribI3uiv;
    public static void glVertexAttribI3uiv(uint index, in uint v) => _glVertexAttribI3uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4UIVPROC(uint index, in uint v);
    private static PFNGLVERTEXATTRIBI4UIVPROC _glVertexAttribI4uiv;
    public static void glVertexAttribI4uiv(uint index, in uint v) => _glVertexAttribI4uiv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4BVPROC(uint index, in sbyte v);
    private static PFNGLVERTEXATTRIBI4BVPROC _glVertexAttribI4bv;
    public static void glVertexAttribI4bv(uint index, in sbyte v) => _glVertexAttribI4bv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4SVPROC(uint index, in short v);
    private static PFNGLVERTEXATTRIBI4SVPROC _glVertexAttribI4sv;
    public static void glVertexAttribI4sv(uint index, in short v) => _glVertexAttribI4sv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4UBVPROC(uint index, IntPtr v);
    private static PFNGLVERTEXATTRIBI4UBVPROC _glVertexAttribI4ubv;
    public static void glVertexAttribI4ubv(uint index, IntPtr v) => _glVertexAttribI4ubv(index, v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBI4USVPROC(uint index, in ushort v);
    private static PFNGLVERTEXATTRIBI4USVPROC _glVertexAttribI4usv;
    public static void glVertexAttribI4usv(uint index, in ushort v) => _glVertexAttribI4usv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMUIVPROC(uint program, int location, out uint @params);
    private static PFNGLGETUNIFORMUIVPROC _glGetUniformuiv;
    public static void glGetUniformuiv(uint program, int location, out uint @params) => _glGetUniformuiv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDFRAGDATALOCATIONPROC(uint program, uint color, string name);
    private static PFNGLBINDFRAGDATALOCATIONPROC _glBindFragDataLocation;
    public static void glBindFragDataLocation(uint program, uint color, string name) => _glBindFragDataLocation(program, color, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETFRAGDATALOCATIONPROC(uint program, string name);
    private static PFNGLGETFRAGDATALOCATIONPROC _glGetFragDataLocation;
    public static int glGetFragDataLocation(uint program, string name) => _glGetFragDataLocation(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1UIPROC(int location, uint v0);
    private static PFNGLUNIFORM1UIPROC _glUniform1ui;
    public static void glUniform1ui(int location, uint v0) => _glUniform1ui(location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2UIPROC(int location, uint v0, uint v1);
    private static PFNGLUNIFORM2UIPROC _glUniform2ui;
    public static void glUniform2ui(int location, uint v0, uint v1) => _glUniform2ui(location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3UIPROC(int location, uint v0, uint v1, uint v2);
    private static PFNGLUNIFORM3UIPROC _glUniform3ui;
    public static void glUniform3ui(int location, uint v0, uint v1, uint v2) => _glUniform3ui(location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4UIPROC(int location, uint v0, uint v1, uint v2, uint v3);
    private static PFNGLUNIFORM4UIPROC _glUniform4ui;
    public static void glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3) => _glUniform4ui(location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1UIVPROC(int location, int count, in uint value);
    private static PFNGLUNIFORM1UIVPROC _glUniform1uiv;
    public static void glUniform1uiv(int location, int count, in uint value) => _glUniform1uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2UIVPROC(int location, int count, in uint value);
    private static PFNGLUNIFORM2UIVPROC _glUniform2uiv;
    public static void glUniform2uiv(int location, int count, in uint value) => _glUniform2uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3UIVPROC(int location, int count, in uint value);
    private static PFNGLUNIFORM3UIVPROC _glUniform3uiv;
    public static void glUniform3uiv(int location, int count, in uint value) => _glUniform3uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4UIVPROC(int location, int count, in uint value);
    private static PFNGLUNIFORM4UIVPROC _glUniform4uiv;
    public static void glUniform4uiv(int location, int count, in uint value) => _glUniform4uiv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIIVPROC(uint target, uint pname, in int @params);
    private static PFNGLTEXPARAMETERIIVPROC _glTexParameterIiv;
    public static void glTexParameterIiv(uint target, uint pname, in int @params) => _glTexParameterIiv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXPARAMETERIUIVPROC(uint target, uint pname, in uint @params);
    private static PFNGLTEXPARAMETERIUIVPROC _glTexParameterIuiv;
    public static void glTexParameterIuiv(uint target, uint pname, in uint @params) => _glTexParameterIuiv(target, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERIIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETTEXPARAMETERIIVPROC _glGetTexParameterIiv;
    public static void glGetTexParameterIiv(uint target, uint pname, out int @params) => _glGetTexParameterIiv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXPARAMETERIUIVPROC(uint target, uint pname, out uint @params);
    private static PFNGLGETTEXPARAMETERIUIVPROC _glGetTexParameterIuiv;
    public static void glGetTexParameterIuiv(uint target, uint pname, out uint @params) => _glGetTexParameterIuiv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERIVPROC(uint buffer, int drawbuffer, in int value);
    private static PFNGLCLEARBUFFERIVPROC _glClearBufferiv;
    public static void glClearBufferiv(uint buffer, int drawbuffer, in int value) => _glClearBufferiv(buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERUIVPROC(uint buffer, int drawbuffer, in uint value);
    private static PFNGLCLEARBUFFERUIVPROC _glClearBufferuiv;
    public static void glClearBufferuiv(uint buffer, int drawbuffer, in uint value) => _glClearBufferuiv(buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERFVPROC(uint buffer, int drawbuffer, in float value);
    private static PFNGLCLEARBUFFERFVPROC _glClearBufferfv;
    public static void glClearBufferfv(uint buffer, int drawbuffer, in float value) => _glClearBufferfv(buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERFIPROC(uint buffer, int drawbuffer, float depth, int stencil);
    private static PFNGLCLEARBUFFERFIPROC _glClearBufferfi;
    public static void glClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil) => _glClearBufferfi(buffer, drawbuffer, depth, stencil);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLGETSTRINGIPROC(uint name, uint index);
    private static PFNGLGETSTRINGIPROC _glGetStringi;
    public static IntPtr glGetStringi(uint name, uint index) => _glGetStringi(name, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISRENDERBUFFERPROC(uint renderbuffer);
    private static PFNGLISRENDERBUFFERPROC _glIsRenderbuffer;
    public static bool glIsRenderbuffer(uint renderbuffer) => _glIsRenderbuffer(renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDRENDERBUFFERPROC(uint target, uint renderbuffer);
    private static PFNGLBINDRENDERBUFFERPROC _glBindRenderbuffer;
    public static void glBindRenderbuffer(uint target, uint renderbuffer) => _glBindRenderbuffer(target, renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETERENDERBUFFERSPROC(int n, in uint renderbuffers);
    private static PFNGLDELETERENDERBUFFERSPROC _glDeleteRenderbuffers;
    public static void glDeleteRenderbuffers(int n, in uint renderbuffers) => _glDeleteRenderbuffers(n, in renderbuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENRENDERBUFFERSPROC(int n, out uint renderbuffers);
    private static PFNGLGENRENDERBUFFERSPROC _glGenRenderbuffers;
    public static void glGenRenderbuffers(int n, out uint renderbuffers) => _glGenRenderbuffers(n, out renderbuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRENDERBUFFERSTORAGEPROC(uint target, uint internalformat, int width, int height);
    private static PFNGLRENDERBUFFERSTORAGEPROC _glRenderbufferStorage;
    public static void glRenderbufferStorage(uint target, uint internalformat, int width, int height) => _glRenderbufferStorage(target, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETRENDERBUFFERPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETRENDERBUFFERPARAMETERIVPROC _glGetRenderbufferParameteriv;
    public static void glGetRenderbufferParameteriv(uint target, uint pname, out int @params) => _glGetRenderbufferParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISFRAMEBUFFERPROC(uint framebuffer);
    private static PFNGLISFRAMEBUFFERPROC _glIsFramebuffer;
    public static bool glIsFramebuffer(uint framebuffer) => _glIsFramebuffer(framebuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDFRAMEBUFFERPROC(uint target, uint framebuffer);
    private static PFNGLBINDFRAMEBUFFERPROC _glBindFramebuffer;
    public static void glBindFramebuffer(uint target, uint framebuffer) => _glBindFramebuffer(target, framebuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEFRAMEBUFFERSPROC(int n, in uint framebuffers);
    private static PFNGLDELETEFRAMEBUFFERSPROC _glDeleteFramebuffers;
    public static void glDeleteFramebuffers(int n, in uint framebuffers) => _glDeleteFramebuffers(n, in framebuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENFRAMEBUFFERSPROC(int n, out uint framebuffers);
    private static PFNGLGENFRAMEBUFFERSPROC _glGenFramebuffers;
    public static void glGenFramebuffers(int n, out uint framebuffers) => _glGenFramebuffers(n, out framebuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCHECKFRAMEBUFFERSTATUSPROC(uint target);
    private static PFNGLCHECKFRAMEBUFFERSTATUSPROC _glCheckFramebufferStatus;
    public static uint glCheckFramebufferStatus(uint target) => _glCheckFramebufferStatus(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURE1DPROC(uint target, uint attachment, uint textarget, uint texture, int level);
    private static PFNGLFRAMEBUFFERTEXTURE1DPROC _glFramebufferTexture1D;
    public static void glFramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level) => _glFramebufferTexture1D(target, attachment, textarget, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURE2DPROC(uint target, uint attachment, uint textarget, uint texture, int level);
    private static PFNGLFRAMEBUFFERTEXTURE2DPROC _glFramebufferTexture2D;
    public static void glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level) => _glFramebufferTexture2D(target, attachment, textarget, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURE3DPROC(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset);
    private static PFNGLFRAMEBUFFERTEXTURE3DPROC _glFramebufferTexture3D;
    public static void glFramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset) => _glFramebufferTexture3D(target, attachment, textarget, texture, level, zoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERRENDERBUFFERPROC(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
    private static PFNGLFRAMEBUFFERRENDERBUFFERPROC _glFramebufferRenderbuffer;
    public static void glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer) => _glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC(uint target, uint attachment, uint pname, out int @params);
    private static PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC _glGetFramebufferAttachmentParameteriv;
    public static void glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, out int @params) => _glGetFramebufferAttachmentParameteriv(target, attachment, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENERATEMIPMAPPROC(uint target);
    private static PFNGLGENERATEMIPMAPPROC _glGenerateMipmap;
    public static void glGenerateMipmap(uint target) => _glGenerateMipmap(target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLITFRAMEBUFFERPROC(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
    private static PFNGLBLITFRAMEBUFFERPROC _glBlitFramebuffer;
    public static void glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) => _glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height);
    private static PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC _glRenderbufferStorageMultisample;
    public static void glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height) => _glRenderbufferStorageMultisample(target, samples, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTURELAYERPROC(uint target, uint attachment, uint texture, int level, int layer);
    private static PFNGLFRAMEBUFFERTEXTURELAYERPROC _glFramebufferTextureLayer;
    public static void glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer) => _glFramebufferTextureLayer(target, attachment, texture, level, layer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPBUFFERRANGEPROC(uint target, ulong offset, ulong length, uint access);
    private static PFNGLMAPBUFFERRANGEPROC _glMapBufferRange;
    public static IntPtr glMapBufferRange(uint target, ulong offset, ulong length, uint access) => _glMapBufferRange(target, offset, length, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFLUSHMAPPEDBUFFERRANGEPROC(uint target, ulong offset, ulong length);
    private static PFNGLFLUSHMAPPEDBUFFERRANGEPROC _glFlushMappedBufferRange;
    public static void glFlushMappedBufferRange(uint target, ulong offset, ulong length) => _glFlushMappedBufferRange(target, offset, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXARRAYPROC(uint array);
    private static PFNGLBINDVERTEXARRAYPROC _glBindVertexArray;
    public static void glBindVertexArray(uint array) => _glBindVertexArray(array);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEVERTEXARRAYSPROC(int n, in uint arrays);
    private static PFNGLDELETEVERTEXARRAYSPROC _glDeleteVertexArrays;
    public static void glDeleteVertexArrays(int n, in uint arrays) => _glDeleteVertexArrays(n, in arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENVERTEXARRAYSPROC(int n, out uint arrays);
    private static PFNGLGENVERTEXARRAYSPROC _glGenVertexArrays;
    public static void glGenVertexArrays(int n, out uint arrays) => _glGenVertexArrays(n, out arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISVERTEXARRAYPROC(uint array);
    private static PFNGLISVERTEXARRAYPROC _glIsVertexArray;
    public static bool glIsVertexArray(uint array) => _glIsVertexArray(array);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINSTANCEDPROC(uint mode, int first, int count, int instancecount);
    private static PFNGLDRAWARRAYSINSTANCEDPROC _glDrawArraysInstanced;
    public static void glDrawArraysInstanced(uint mode, int first, int count, int instancecount) => _glDrawArraysInstanced(mode, first, count, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDPROC(uint mode, int count, uint type, IntPtr indices, int instancecount);
    private static PFNGLDRAWELEMENTSINSTANCEDPROC _glDrawElementsInstanced;
    public static void glDrawElementsInstanced(uint mode, int count, uint type, IntPtr indices, int instancecount) => _glDrawElementsInstanced(mode, count, type, indices, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXBUFFERPROC(uint target, uint internalformat, uint buffer);
    private static PFNGLTEXBUFFERPROC _glTexBuffer;
    public static void glTexBuffer(uint target, uint internalformat, uint buffer) => _glTexBuffer(target, internalformat, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPRIMITIVERESTARTINDEXPROC(uint index);
    private static PFNGLPRIMITIVERESTARTINDEXPROC _glPrimitiveRestartIndex;
    public static void glPrimitiveRestartIndex(uint index) => _glPrimitiveRestartIndex(index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYBUFFERSUBDATAPROC(uint readTarget, uint writeTarget, ulong readOffset, ulong writeOffset, ulong size);
    private static PFNGLCOPYBUFFERSUBDATAPROC _glCopyBufferSubData;
    public static void glCopyBufferSubData(uint readTarget, uint writeTarget, ulong readOffset, ulong writeOffset, ulong size) => _glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMINDICESPROC(uint program, int uniformCount, string uniformNames, out uint uniformIndices);
    private static PFNGLGETUNIFORMINDICESPROC _glGetUniformIndices;
    public static void glGetUniformIndices(uint program, int uniformCount, string uniformNames, out uint uniformIndices) => _glGetUniformIndices(program, uniformCount, uniformNames, out uniformIndices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMSIVPROC(uint program, int uniformCount, in uint uniformIndices, uint pname, out int @params);
    private static PFNGLGETACTIVEUNIFORMSIVPROC _glGetActiveUniformsiv;
    public static void glGetActiveUniformsiv(uint program, int uniformCount, in uint uniformIndices, uint pname, out int @params) => _glGetActiveUniformsiv(program, uniformCount, in uniformIndices, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMNAMEPROC(uint program, uint uniformIndex, int bufSize, out int length, string uniformName);
    private static PFNGLGETACTIVEUNIFORMNAMEPROC _glGetActiveUniformName;
    public static void glGetActiveUniformName(uint program, uint uniformIndex, int bufSize, out int length, string uniformName) => _glGetActiveUniformName(program, uniformIndex, bufSize, out length, uniformName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETUNIFORMBLOCKINDEXPROC(uint program, string uniformBlockName);
    private static PFNGLGETUNIFORMBLOCKINDEXPROC _glGetUniformBlockIndex;
    public static uint glGetUniformBlockIndex(uint program, string uniformBlockName) => _glGetUniformBlockIndex(program, uniformBlockName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMBLOCKIVPROC(uint program, uint uniformBlockIndex, uint pname, out int @params);
    private static PFNGLGETACTIVEUNIFORMBLOCKIVPROC _glGetActiveUniformBlockiv;
    public static void glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, out int @params) => _glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC(uint program, uint uniformBlockIndex, int bufSize, out int length, string uniformBlockName);
    private static PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC _glGetActiveUniformBlockName;
    public static void glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, out int length, string uniformBlockName) => _glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, out length, uniformBlockName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMBLOCKBINDINGPROC(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
    private static PFNGLUNIFORMBLOCKBINDINGPROC _glUniformBlockBinding;
    public static void glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => _glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSBASEVERTEXPROC(uint mode, int count, uint type, IntPtr indices, int basevertex);
    private static PFNGLDRAWELEMENTSBASEVERTEXPROC _glDrawElementsBaseVertex;
    public static void glDrawElementsBaseVertex(uint mode, int count, uint type, IntPtr indices, int basevertex) => _glDrawElementsBaseVertex(mode, count, type, indices, basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC(uint mode, uint start, uint end, int count, uint type, IntPtr indices, int basevertex);
    private static PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC _glDrawRangeElementsBaseVertex;
    public static void glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, IntPtr indices, int basevertex) => _glDrawRangeElementsBaseVertex(mode, start, end, count, type, indices, basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex);
    private static PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC _glDrawElementsInstancedBaseVertex;
    public static void glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex) => _glDrawElementsInstancedBaseVertex(mode, count, type, indices, instancecount, basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC(uint mode, in int count, uint type, IntPtr indices, int drawcount, in int basevertex);
    private static PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC _glMultiDrawElementsBaseVertex;
    public static void glMultiDrawElementsBaseVertex(uint mode, in int count, uint type, IntPtr indices, int drawcount, in int basevertex) => _glMultiDrawElementsBaseVertex(mode, in count, type, indices, drawcount, in basevertex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROVOKINGVERTEXPROC(uint mode);
    private static PFNGLPROVOKINGVERTEXPROC _glProvokingVertex;
    public static void glProvokingVertex(uint mode) => _glProvokingVertex(mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLFENCESYNCPROC(uint condition, uint flags);
    private static PFNGLFENCESYNCPROC _glFenceSync;
    public static IntPtr glFenceSync(uint condition, uint flags) => _glFenceSync(condition, flags);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISSYNCPROC(IntPtr sync);
    private static PFNGLISSYNCPROC _glIsSync;
    public static bool glIsSync(IntPtr sync) => _glIsSync(sync);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETESYNCPROC(IntPtr sync);
    private static PFNGLDELETESYNCPROC _glDeleteSync;
    public static void glDeleteSync(IntPtr sync) => _glDeleteSync(sync);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCLIENTWAITSYNCPROC(IntPtr sync, uint flags, ulong timeout);
    private static PFNGLCLIENTWAITSYNCPROC _glClientWaitSync;
    public static uint glClientWaitSync(IntPtr sync, uint flags, ulong timeout) => _glClientWaitSync(sync, flags, timeout);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLWAITSYNCPROC(IntPtr sync, uint flags, ulong timeout);
    private static PFNGLWAITSYNCPROC _glWaitSync;
    public static void glWaitSync(IntPtr sync, uint flags, ulong timeout) => _glWaitSync(sync, flags, timeout);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGER64VPROC(uint pname, out long data);
    private static PFNGLGETINTEGER64VPROC _glGetInteger64v;
    public static void glGetInteger64v(uint pname, out long data) => _glGetInteger64v(pname, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSYNCIVPROC(IntPtr sync, uint pname, int count, out int length, out int values);
    private static PFNGLGETSYNCIVPROC _glGetSynciv;
    public static void glGetSynciv(IntPtr sync, uint pname, int count, out int length, out int values) => _glGetSynciv(sync, pname, count, out length, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTEGER64I_VPROC(uint target, uint index, out long data);
    private static PFNGLGETINTEGER64I_VPROC _glGetInteger64i_v;
    public static void glGetInteger64i_v(uint target, uint index, out long data) => _glGetInteger64i_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETBUFFERPARAMETERI64VPROC(uint target, uint pname, out long @params);
    private static PFNGLGETBUFFERPARAMETERI64VPROC _glGetBufferParameteri64v;
    public static void glGetBufferParameteri64v(uint target, uint pname, out long @params) => _glGetBufferParameteri64v(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERTEXTUREPROC(uint target, uint attachment, uint texture, int level);
    private static PFNGLFRAMEBUFFERTEXTUREPROC _glFramebufferTexture;
    public static void glFramebufferTexture(uint target, uint attachment, uint texture, int level) => _glFramebufferTexture(target, attachment, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE2DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
    private static PFNGLTEXIMAGE2DMULTISAMPLEPROC _glTexImage2DMultisample;
    public static void glTexImage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations) => _glTexImage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXIMAGE3DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
    private static PFNGLTEXIMAGE3DMULTISAMPLEPROC _glTexImage3DMultisample;
    public static void glTexImage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTexImage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETMULTISAMPLEFVPROC(uint pname, uint index, out float val);
    private static PFNGLGETMULTISAMPLEFVPROC _glGetMultisamplefv;
    public static void glGetMultisamplefv(uint pname, uint index, out float val) => _glGetMultisamplefv(pname, index, out val);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLEMASKIPROC(uint maskNumber, uint mask);
    private static PFNGLSAMPLEMASKIPROC _glSampleMaski;
    public static void glSampleMaski(uint maskNumber, uint mask) => _glSampleMaski(maskNumber, mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDFRAGDATALOCATIONINDEXEDPROC(uint program, uint colorNumber, uint index, string name);
    private static PFNGLBINDFRAGDATALOCATIONINDEXEDPROC _glBindFragDataLocationIndexed;
    public static void glBindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name) => _glBindFragDataLocationIndexed(program, colorNumber, index, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETFRAGDATAINDEXPROC(uint program, string name);
    private static PFNGLGETFRAGDATAINDEXPROC _glGetFragDataIndex;
    public static int glGetFragDataIndex(uint program, string name) => _glGetFragDataIndex(program, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENSAMPLERSPROC(int count, out uint samplers);
    private static PFNGLGENSAMPLERSPROC _glGenSamplers;
    public static void glGenSamplers(int count, out uint samplers) => _glGenSamplers(count, out samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETESAMPLERSPROC(int count, in uint samplers);
    private static PFNGLDELETESAMPLERSPROC _glDeleteSamplers;
    public static void glDeleteSamplers(int count, in uint samplers) => _glDeleteSamplers(count, in samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISSAMPLERPROC(uint sampler);
    private static PFNGLISSAMPLERPROC _glIsSampler;
    public static bool glIsSampler(uint sampler) => _glIsSampler(sampler);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDSAMPLERPROC(uint unit, uint sampler);
    private static PFNGLBINDSAMPLERPROC _glBindSampler;
    public static void glBindSampler(uint unit, uint sampler) => _glBindSampler(unit, sampler);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIPROC(uint sampler, uint pname, int param);
    private static PFNGLSAMPLERPARAMETERIPROC _glSamplerParameteri;
    public static void glSamplerParameteri(uint sampler, uint pname, int param) => _glSamplerParameteri(sampler, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIVPROC(uint sampler, uint pname, in int param);
    private static PFNGLSAMPLERPARAMETERIVPROC _glSamplerParameteriv;
    public static void glSamplerParameteriv(uint sampler, uint pname, in int param) => _glSamplerParameteriv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERFPROC(uint sampler, uint pname, float param);
    private static PFNGLSAMPLERPARAMETERFPROC _glSamplerParameterf;
    public static void glSamplerParameterf(uint sampler, uint pname, float param) => _glSamplerParameterf(sampler, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERFVPROC(uint sampler, uint pname, in float param);
    private static PFNGLSAMPLERPARAMETERFVPROC _glSamplerParameterfv;
    public static void glSamplerParameterfv(uint sampler, uint pname, in float param) => _glSamplerParameterfv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIIVPROC(uint sampler, uint pname, in int param);
    private static PFNGLSAMPLERPARAMETERIIVPROC _glSamplerParameterIiv;
    public static void glSamplerParameterIiv(uint sampler, uint pname, in int param) => _glSamplerParameterIiv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSAMPLERPARAMETERIUIVPROC(uint sampler, uint pname, in uint param);
    private static PFNGLSAMPLERPARAMETERIUIVPROC _glSamplerParameterIuiv;
    public static void glSamplerParameterIuiv(uint sampler, uint pname, in uint param) => _glSamplerParameterIuiv(sampler, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERIVPROC(uint sampler, uint pname, out int @params);
    private static PFNGLGETSAMPLERPARAMETERIVPROC _glGetSamplerParameteriv;
    public static void glGetSamplerParameteriv(uint sampler, uint pname, out int @params) => _glGetSamplerParameteriv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERIIVPROC(uint sampler, uint pname, out int @params);
    private static PFNGLGETSAMPLERPARAMETERIIVPROC _glGetSamplerParameterIiv;
    public static void glGetSamplerParameterIiv(uint sampler, uint pname, out int @params) => _glGetSamplerParameterIiv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERFVPROC(uint sampler, uint pname, out float @params);
    private static PFNGLGETSAMPLERPARAMETERFVPROC _glGetSamplerParameterfv;
    public static void glGetSamplerParameterfv(uint sampler, uint pname, out float @params) => _glGetSamplerParameterfv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSAMPLERPARAMETERIUIVPROC(uint sampler, uint pname, out uint @params);
    private static PFNGLGETSAMPLERPARAMETERIUIVPROC _glGetSamplerParameterIuiv;
    public static void glGetSamplerParameterIuiv(uint sampler, uint pname, out uint @params) => _glGetSamplerParameterIuiv(sampler, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLQUERYCOUNTERPROC(uint id, uint target);
    private static PFNGLQUERYCOUNTERPROC _glQueryCounter;
    public static void glQueryCounter(uint id, uint target) => _glQueryCounter(id, target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTI64VPROC(uint id, uint pname, out long @params);
    private static PFNGLGETQUERYOBJECTI64VPROC _glGetQueryObjecti64v;
    public static void glGetQueryObjecti64v(uint id, uint pname, out long @params) => _glGetQueryObjecti64v(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYOBJECTUI64VPROC(uint id, uint pname, out ulong @params);
    private static PFNGLGETQUERYOBJECTUI64VPROC _glGetQueryObjectui64v;
    public static void glGetQueryObjectui64v(uint id, uint pname, out ulong @params) => _glGetQueryObjectui64v(id, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBDIVISORPROC(uint index, uint divisor);
    private static PFNGLVERTEXATTRIBDIVISORPROC _glVertexAttribDivisor;
    public static void glVertexAttribDivisor(uint index, uint divisor) => _glVertexAttribDivisor(index, divisor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP1UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP1UIPROC _glVertexAttribP1ui;
    public static void glVertexAttribP1ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP1ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP1UIVPROC(uint index, uint type, bool normalized, in uint value);
    private static PFNGLVERTEXATTRIBP1UIVPROC _glVertexAttribP1uiv;
    public static void glVertexAttribP1uiv(uint index, uint type, bool normalized, in uint value) => _glVertexAttribP1uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP2UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP2UIPROC _glVertexAttribP2ui;
    public static void glVertexAttribP2ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP2ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP2UIVPROC(uint index, uint type, bool normalized, in uint value);
    private static PFNGLVERTEXATTRIBP2UIVPROC _glVertexAttribP2uiv;
    public static void glVertexAttribP2uiv(uint index, uint type, bool normalized, in uint value) => _glVertexAttribP2uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP3UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP3UIPROC _glVertexAttribP3ui;
    public static void glVertexAttribP3ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP3ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP3UIVPROC(uint index, uint type, bool normalized, in uint value);
    private static PFNGLVERTEXATTRIBP3UIVPROC _glVertexAttribP3uiv;
    public static void glVertexAttribP3uiv(uint index, uint type, bool normalized, in uint value) => _glVertexAttribP3uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP4UIPROC(uint index, uint type, bool normalized, uint value);
    private static PFNGLVERTEXATTRIBP4UIPROC _glVertexAttribP4ui;
    public static void glVertexAttribP4ui(uint index, uint type, bool normalized, uint value) => _glVertexAttribP4ui(index, type, normalized, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBP4UIVPROC(uint index, uint type, bool normalized, in uint value);
    private static PFNGLVERTEXATTRIBP4UIVPROC _glVertexAttribP4uiv;
    public static void glVertexAttribP4uiv(uint index, uint type, bool normalized, in uint value) => _glVertexAttribP4uiv(index, type, normalized, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP2UIPROC(uint type, uint value);
    private static PFNGLVERTEXP2UIPROC _glVertexP2ui;
    public static void glVertexP2ui(uint type, uint value) => _glVertexP2ui(type, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP2UIVPROC(uint type, in uint value);
    private static PFNGLVERTEXP2UIVPROC _glVertexP2uiv;
    public static void glVertexP2uiv(uint type, in uint value) => _glVertexP2uiv(type, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP3UIPROC(uint type, uint value);
    private static PFNGLVERTEXP3UIPROC _glVertexP3ui;
    public static void glVertexP3ui(uint type, uint value) => _glVertexP3ui(type, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP3UIVPROC(uint type, in uint value);
    private static PFNGLVERTEXP3UIVPROC _glVertexP3uiv;
    public static void glVertexP3uiv(uint type, in uint value) => _glVertexP3uiv(type, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP4UIPROC(uint type, uint value);
    private static PFNGLVERTEXP4UIPROC _glVertexP4ui;
    public static void glVertexP4ui(uint type, uint value) => _glVertexP4ui(type, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXP4UIVPROC(uint type, in uint value);
    private static PFNGLVERTEXP4UIVPROC _glVertexP4uiv;
    public static void glVertexP4uiv(uint type, in uint value) => _glVertexP4uiv(type, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP1UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP1UIPROC _glTexCoordP1ui;
    public static void glTexCoordP1ui(uint type, uint coords) => _glTexCoordP1ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP1UIVPROC(uint type, in uint coords);
    private static PFNGLTEXCOORDP1UIVPROC _glTexCoordP1uiv;
    public static void glTexCoordP1uiv(uint type, in uint coords) => _glTexCoordP1uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP2UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP2UIPROC _glTexCoordP2ui;
    public static void glTexCoordP2ui(uint type, uint coords) => _glTexCoordP2ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP2UIVPROC(uint type, in uint coords);
    private static PFNGLTEXCOORDP2UIVPROC _glTexCoordP2uiv;
    public static void glTexCoordP2uiv(uint type, in uint coords) => _glTexCoordP2uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP3UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP3UIPROC _glTexCoordP3ui;
    public static void glTexCoordP3ui(uint type, uint coords) => _glTexCoordP3ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP3UIVPROC(uint type, in uint coords);
    private static PFNGLTEXCOORDP3UIVPROC _glTexCoordP3uiv;
    public static void glTexCoordP3uiv(uint type, in uint coords) => _glTexCoordP3uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP4UIPROC(uint type, uint coords);
    private static PFNGLTEXCOORDP4UIPROC _glTexCoordP4ui;
    public static void glTexCoordP4ui(uint type, uint coords) => _glTexCoordP4ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXCOORDP4UIVPROC(uint type, in uint coords);
    private static PFNGLTEXCOORDP4UIVPROC _glTexCoordP4uiv;
    public static void glTexCoordP4uiv(uint type, in uint coords) => _glTexCoordP4uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP1UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP1UIPROC _glMultiTexCoordP1ui;
    public static void glMultiTexCoordP1ui(uint texture, uint type, uint coords) => _glMultiTexCoordP1ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP1UIVPROC(uint texture, uint type, in uint coords);
    private static PFNGLMULTITEXCOORDP1UIVPROC _glMultiTexCoordP1uiv;
    public static void glMultiTexCoordP1uiv(uint texture, uint type, in uint coords) => _glMultiTexCoordP1uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP2UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP2UIPROC _glMultiTexCoordP2ui;
    public static void glMultiTexCoordP2ui(uint texture, uint type, uint coords) => _glMultiTexCoordP2ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP2UIVPROC(uint texture, uint type, in uint coords);
    private static PFNGLMULTITEXCOORDP2UIVPROC _glMultiTexCoordP2uiv;
    public static void glMultiTexCoordP2uiv(uint texture, uint type, in uint coords) => _glMultiTexCoordP2uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP3UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP3UIPROC _glMultiTexCoordP3ui;
    public static void glMultiTexCoordP3ui(uint texture, uint type, uint coords) => _glMultiTexCoordP3ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP3UIVPROC(uint texture, uint type, in uint coords);
    private static PFNGLMULTITEXCOORDP3UIVPROC _glMultiTexCoordP3uiv;
    public static void glMultiTexCoordP3uiv(uint texture, uint type, in uint coords) => _glMultiTexCoordP3uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP4UIPROC(uint texture, uint type, uint coords);
    private static PFNGLMULTITEXCOORDP4UIPROC _glMultiTexCoordP4ui;
    public static void glMultiTexCoordP4ui(uint texture, uint type, uint coords) => _glMultiTexCoordP4ui(texture, type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTITEXCOORDP4UIVPROC(uint texture, uint type, in uint coords);
    private static PFNGLMULTITEXCOORDP4UIVPROC _glMultiTexCoordP4uiv;
    public static void glMultiTexCoordP4uiv(uint texture, uint type, in uint coords) => _glMultiTexCoordP4uiv(texture, type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNORMALP3UIPROC(uint type, uint coords);
    private static PFNGLNORMALP3UIPROC _glNormalP3ui;
    public static void glNormalP3ui(uint type, uint coords) => _glNormalP3ui(type, coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNORMALP3UIVPROC(uint type, in uint coords);
    private static PFNGLNORMALP3UIVPROC _glNormalP3uiv;
    public static void glNormalP3uiv(uint type, in uint coords) => _glNormalP3uiv(type, in coords);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP3UIPROC(uint type, uint color);
    private static PFNGLCOLORP3UIPROC _glColorP3ui;
    public static void glColorP3ui(uint type, uint color) => _glColorP3ui(type, color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP3UIVPROC(uint type, in uint color);
    private static PFNGLCOLORP3UIVPROC _glColorP3uiv;
    public static void glColorP3uiv(uint type, in uint color) => _glColorP3uiv(type, in color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP4UIPROC(uint type, uint color);
    private static PFNGLCOLORP4UIPROC _glColorP4ui;
    public static void glColorP4ui(uint type, uint color) => _glColorP4ui(type, color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOLORP4UIVPROC(uint type, in uint color);
    private static PFNGLCOLORP4UIVPROC _glColorP4uiv;
    public static void glColorP4uiv(uint type, in uint color) => _glColorP4uiv(type, in color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSECONDARYCOLORP3UIPROC(uint type, uint color);
    private static PFNGLSECONDARYCOLORP3UIPROC _glSecondaryColorP3ui;
    public static void glSecondaryColorP3ui(uint type, uint color) => _glSecondaryColorP3ui(type, color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSECONDARYCOLORP3UIVPROC(uint type, in uint color);
    private static PFNGLSECONDARYCOLORP3UIVPROC _glSecondaryColorP3uiv;
    public static void glSecondaryColorP3uiv(uint type, in uint color) => _glSecondaryColorP3uiv(type, in color);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMINSAMPLESHADINGPROC(float value);
    private static PFNGLMINSAMPLESHADINGPROC _glMinSampleShading;
    public static void glMinSampleShading(float value) => _glMinSampleShading(value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONIPROC(uint buf, uint mode);
    private static PFNGLBLENDEQUATIONIPROC _glBlendEquationi;
    public static void glBlendEquationi(uint buf, uint mode) => _glBlendEquationi(buf, mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDEQUATIONSEPARATEIPROC(uint buf, uint modeRGB, uint modeAlpha);
    private static PFNGLBLENDEQUATIONSEPARATEIPROC _glBlendEquationSeparatei;
    public static void glBlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha) => _glBlendEquationSeparatei(buf, modeRGB, modeAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCIPROC(uint buf, uint src, uint dst);
    private static PFNGLBLENDFUNCIPROC _glBlendFunci;
    public static void glBlendFunci(uint buf, uint src, uint dst) => _glBlendFunci(buf, src, dst);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLENDFUNCSEPARATEIPROC(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);
    private static PFNGLBLENDFUNCSEPARATEIPROC _glBlendFuncSeparatei;
    public static void glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha) => _glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINDIRECTPROC(uint mode, IntPtr indirect);
    private static PFNGLDRAWARRAYSINDIRECTPROC _glDrawArraysIndirect;
    public static void glDrawArraysIndirect(uint mode, IntPtr indirect) => _glDrawArraysIndirect(mode, indirect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINDIRECTPROC(uint mode, uint type, IntPtr indirect);
    private static PFNGLDRAWELEMENTSINDIRECTPROC _glDrawElementsIndirect;
    public static void glDrawElementsIndirect(uint mode, uint type, IntPtr indirect) => _glDrawElementsIndirect(mode, type, indirect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1DPROC(int location, double x);
    private static PFNGLUNIFORM1DPROC _glUniform1d;
    public static void glUniform1d(int location, double x) => _glUniform1d(location, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2DPROC(int location, double x, double y);
    private static PFNGLUNIFORM2DPROC _glUniform2d;
    public static void glUniform2d(int location, double x, double y) => _glUniform2d(location, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3DPROC(int location, double x, double y, double z);
    private static PFNGLUNIFORM3DPROC _glUniform3d;
    public static void glUniform3d(int location, double x, double y, double z) => _glUniform3d(location, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4DPROC(int location, double x, double y, double z, double w);
    private static PFNGLUNIFORM4DPROC _glUniform4d;
    public static void glUniform4d(int location, double x, double y, double z, double w) => _glUniform4d(location, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM1DVPROC(int location, int count, in double value);
    private static PFNGLUNIFORM1DVPROC _glUniform1dv;
    public static void glUniform1dv(int location, int count, in double value) => _glUniform1dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM2DVPROC(int location, int count, in double value);
    private static PFNGLUNIFORM2DVPROC _glUniform2dv;
    public static void glUniform2dv(int location, int count, in double value) => _glUniform2dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM3DVPROC(int location, int count, in double value);
    private static PFNGLUNIFORM3DVPROC _glUniform3dv;
    public static void glUniform3dv(int location, int count, in double value) => _glUniform3dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORM4DVPROC(int location, int count, in double value);
    private static PFNGLUNIFORM4DVPROC _glUniform4dv;
    public static void glUniform4dv(int location, int count, in double value) => _glUniform4dv(location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX2DVPROC _glUniformMatrix2dv;
    public static void glUniformMatrix2dv(int location, int count, bool transpose, in double value) => _glUniformMatrix2dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX3DVPROC _glUniformMatrix3dv;
    public static void glUniformMatrix3dv(int location, int count, bool transpose, in double value) => _glUniformMatrix3dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX4DVPROC _glUniformMatrix4dv;
    public static void glUniformMatrix4dv(int location, int count, bool transpose, in double value) => _glUniformMatrix4dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X3DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX2X3DVPROC _glUniformMatrix2x3dv;
    public static void glUniformMatrix2x3dv(int location, int count, bool transpose, in double value) => _glUniformMatrix2x3dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX2X4DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX2X4DVPROC _glUniformMatrix2x4dv;
    public static void glUniformMatrix2x4dv(int location, int count, bool transpose, in double value) => _glUniformMatrix2x4dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X2DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX3X2DVPROC _glUniformMatrix3x2dv;
    public static void glUniformMatrix3x2dv(int location, int count, bool transpose, in double value) => _glUniformMatrix3x2dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX3X4DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX3X4DVPROC _glUniformMatrix3x4dv;
    public static void glUniformMatrix3x4dv(int location, int count, bool transpose, in double value) => _glUniformMatrix3x4dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X2DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX4X2DVPROC _glUniformMatrix4x2dv;
    public static void glUniformMatrix4x2dv(int location, int count, bool transpose, in double value) => _glUniformMatrix4x2dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMMATRIX4X3DVPROC(int location, int count, bool transpose, in double value);
    private static PFNGLUNIFORMMATRIX4X3DVPROC _glUniformMatrix4x3dv;
    public static void glUniformMatrix4x3dv(int location, int count, bool transpose, in double value) => _glUniformMatrix4x3dv(location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMDVPROC(uint program, int location, out double @params);
    private static PFNGLGETUNIFORMDVPROC _glGetUniformdv;
    public static void glGetUniformdv(uint program, int location, out double @params) => _glGetUniformdv(program, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC(uint program, uint shadertype, string name);
    private static PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC _glGetSubroutineUniformLocation;
    public static int glGetSubroutineUniformLocation(uint program, uint shadertype, string name) => _glGetSubroutineUniformLocation(program, shadertype, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETSUBROUTINEINDEXPROC(uint program, uint shadertype, string name);
    private static PFNGLGETSUBROUTINEINDEXPROC _glGetSubroutineIndex;
    public static uint glGetSubroutineIndex(uint program, uint shadertype, string name) => _glGetSubroutineIndex(program, shadertype, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC(uint program, uint shadertype, uint index, uint pname, out int values);
    private static PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC _glGetActiveSubroutineUniformiv;
    public static void glGetActiveSubroutineUniformiv(uint program, uint shadertype, uint index, uint pname, out int values) => _glGetActiveSubroutineUniformiv(program, shadertype, index, pname, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC(uint program, uint shadertype, uint index, int bufSize, out int length, string name);
    private static PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC _glGetActiveSubroutineUniformName;
    public static void glGetActiveSubroutineUniformName(uint program, uint shadertype, uint index, int bufSize, out int length, string name) => _glGetActiveSubroutineUniformName(program, shadertype, index, bufSize, out length, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVESUBROUTINENAMEPROC(uint program, uint shadertype, uint index, int bufSize, out int length, string name);
    private static PFNGLGETACTIVESUBROUTINENAMEPROC _glGetActiveSubroutineName;
    public static void glGetActiveSubroutineName(uint program, uint shadertype, uint index, int bufSize, out int length, string name) => _glGetActiveSubroutineName(program, shadertype, index, bufSize, out length, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUNIFORMSUBROUTINESUIVPROC(uint shadertype, int count, in uint indices);
    private static PFNGLUNIFORMSUBROUTINESUIVPROC _glUniformSubroutinesuiv;
    public static void glUniformSubroutinesuiv(uint shadertype, int count, in uint indices) => _glUniformSubroutinesuiv(shadertype, count, in indices);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETUNIFORMSUBROUTINEUIVPROC(uint shadertype, int location, out uint @params);
    private static PFNGLGETUNIFORMSUBROUTINEUIVPROC _glGetUniformSubroutineuiv;
    public static void glGetUniformSubroutineuiv(uint shadertype, int location, out uint @params) => _glGetUniformSubroutineuiv(shadertype, location, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMSTAGEIVPROC(uint program, uint shadertype, uint pname, out int values);
    private static PFNGLGETPROGRAMSTAGEIVPROC _glGetProgramStageiv;
    public static void glGetProgramStageiv(uint program, uint shadertype, uint pname, out int values) => _glGetProgramStageiv(program, shadertype, pname, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPATCHPARAMETERIPROC(uint pname, int value);
    private static PFNGLPATCHPARAMETERIPROC _glPatchParameteri;
    public static void glPatchParameteri(uint pname, int value) => _glPatchParameteri(pname, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPATCHPARAMETERFVPROC(uint pname, in float values);
    private static PFNGLPATCHPARAMETERFVPROC _glPatchParameterfv;
    public static void glPatchParameterfv(uint pname, in float values) => _glPatchParameterfv(pname, in values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTRANSFORMFEEDBACKPROC(uint target, uint id);
    private static PFNGLBINDTRANSFORMFEEDBACKPROC _glBindTransformFeedback;
    public static void glBindTransformFeedback(uint target, uint id) => _glBindTransformFeedback(target, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETETRANSFORMFEEDBACKSPROC(int n, in uint ids);
    private static PFNGLDELETETRANSFORMFEEDBACKSPROC _glDeleteTransformFeedbacks;
    public static void glDeleteTransformFeedbacks(int n, in uint ids) => _glDeleteTransformFeedbacks(n, in ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENTRANSFORMFEEDBACKSPROC(int n, out uint ids);
    private static PFNGLGENTRANSFORMFEEDBACKSPROC _glGenTransformFeedbacks;
    public static void glGenTransformFeedbacks(int n, out uint ids) => _glGenTransformFeedbacks(n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISTRANSFORMFEEDBACKPROC(uint id);
    private static PFNGLISTRANSFORMFEEDBACKPROC _glIsTransformFeedback;
    public static bool glIsTransformFeedback(uint id) => _glIsTransformFeedback(id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPAUSETRANSFORMFEEDBACKPROC();
    private static PFNGLPAUSETRANSFORMFEEDBACKPROC _glPauseTransformFeedback;
    public static void glPauseTransformFeedback() => _glPauseTransformFeedback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRESUMETRANSFORMFEEDBACKPROC();
    private static PFNGLRESUMETRANSFORMFEEDBACKPROC _glResumeTransformFeedback;
    public static void glResumeTransformFeedback() => _glResumeTransformFeedback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKPROC(uint mode, uint id);
    private static PFNGLDRAWTRANSFORMFEEDBACKPROC _glDrawTransformFeedback;
    public static void glDrawTransformFeedback(uint mode, uint id) => _glDrawTransformFeedback(mode, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC(uint mode, uint id, uint stream);
    private static PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC _glDrawTransformFeedbackStream;
    public static void glDrawTransformFeedbackStream(uint mode, uint id, uint stream) => _glDrawTransformFeedbackStream(mode, id, stream);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBEGINQUERYINDEXEDPROC(uint target, uint index, uint id);
    private static PFNGLBEGINQUERYINDEXEDPROC _glBeginQueryIndexed;
    public static void glBeginQueryIndexed(uint target, uint index, uint id) => _glBeginQueryIndexed(target, index, id);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENDQUERYINDEXEDPROC(uint target, uint index);
    private static PFNGLENDQUERYINDEXEDPROC _glEndQueryIndexed;
    public static void glEndQueryIndexed(uint target, uint index) => _glEndQueryIndexed(target, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYINDEXEDIVPROC(uint target, uint index, uint pname, out int @params);
    private static PFNGLGETQUERYINDEXEDIVPROC _glGetQueryIndexediv;
    public static void glGetQueryIndexediv(uint target, uint index, uint pname, out int @params) => _glGetQueryIndexediv(target, index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLRELEASESHADERCOMPILERPROC();
    private static PFNGLRELEASESHADERCOMPILERPROC _glReleaseShaderCompiler;
    public static void glReleaseShaderCompiler() => _glReleaseShaderCompiler();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERBINARYPROC(int count, in uint shaders, uint binaryFormat, IntPtr binary, int length);
    private static PFNGLSHADERBINARYPROC _glShaderBinary;
    public static void glShaderBinary(int count, in uint shaders, uint binaryFormat, IntPtr binary, int length) => _glShaderBinary(count, in shaders, binaryFormat, binary, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETSHADERPRECISIONFORMATPROC(uint shadertype, uint precisiontype, out int range, out int precision);
    private static PFNGLGETSHADERPRECISIONFORMATPROC _glGetShaderPrecisionFormat;
    public static void glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, out int range, out int precision) => _glGetShaderPrecisionFormat(shadertype, precisiontype, out range, out precision);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEFPROC(float n, float f);
    private static PFNGLDEPTHRANGEFPROC _glDepthRangef;
    public static void glDepthRangef(float n, float f) => _glDepthRangef(n, f);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARDEPTHFPROC(float d);
    private static PFNGLCLEARDEPTHFPROC _glClearDepthf;
    public static void glClearDepthf(float d) => _glClearDepthf(d);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMBINARYPROC(uint program, int bufSize, out int length, out uint binaryFormat, IntPtr binary);
    private static PFNGLGETPROGRAMBINARYPROC _glGetProgramBinary;
    public static void glGetProgramBinary(uint program, int bufSize, out int length, out uint binaryFormat, IntPtr binary) => _glGetProgramBinary(program, bufSize, out length, out binaryFormat, binary);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMBINARYPROC(uint program, uint binaryFormat, IntPtr binary, int length);
    private static PFNGLPROGRAMBINARYPROC _glProgramBinary;
    public static void glProgramBinary(uint program, uint binaryFormat, IntPtr binary, int length) => _glProgramBinary(program, binaryFormat, binary, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMPARAMETERIPROC(uint program, uint pname, int value);
    private static PFNGLPROGRAMPARAMETERIPROC _glProgramParameteri;
    public static void glProgramParameteri(uint program, uint pname, int value) => _glProgramParameteri(program, pname, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLUSEPROGRAMSTAGESPROC(uint pipeline, uint stages, uint program);
    private static PFNGLUSEPROGRAMSTAGESPROC _glUseProgramStages;
    public static void glUseProgramStages(uint pipeline, uint stages, uint program) => _glUseProgramStages(pipeline, stages, program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLACTIVESHADERPROGRAMPROC(uint pipeline, uint program);
    private static PFNGLACTIVESHADERPROGRAMPROC _glActiveShaderProgram;
    public static void glActiveShaderProgram(uint pipeline, uint program) => _glActiveShaderProgram(pipeline, program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCREATESHADERPROGRAMVPROC(uint type, int count, string strings);
    private static PFNGLCREATESHADERPROGRAMVPROC _glCreateShaderProgramv;
    public static uint glCreateShaderProgramv(uint type, int count, string strings) => _glCreateShaderProgramv(type, count, strings);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDPROGRAMPIPELINEPROC(uint pipeline);
    private static PFNGLBINDPROGRAMPIPELINEPROC _glBindProgramPipeline;
    public static void glBindProgramPipeline(uint pipeline) => _glBindProgramPipeline(pipeline);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDELETEPROGRAMPIPELINESPROC(int n, in uint pipelines);
    private static PFNGLDELETEPROGRAMPIPELINESPROC _glDeleteProgramPipelines;
    public static void glDeleteProgramPipelines(int n, in uint pipelines) => _glDeleteProgramPipelines(n, in pipelines);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENPROGRAMPIPELINESPROC(int n, out uint pipelines);
    private static PFNGLGENPROGRAMPIPELINESPROC _glGenProgramPipelines;
    public static void glGenProgramPipelines(int n, out uint pipelines) => _glGenProgramPipelines(n, out pipelines);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLISPROGRAMPIPELINEPROC(uint pipeline);
    private static PFNGLISPROGRAMPIPELINEPROC _glIsProgramPipeline;
    public static bool glIsProgramPipeline(uint pipeline) => _glIsProgramPipeline(pipeline);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMPIPELINEIVPROC(uint pipeline, uint pname, out int @params);
    private static PFNGLGETPROGRAMPIPELINEIVPROC _glGetProgramPipelineiv;
    public static void glGetProgramPipelineiv(uint pipeline, uint pname, out int @params) => _glGetProgramPipelineiv(pipeline, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1IPROC(uint program, int location, int v0);
    private static PFNGLPROGRAMUNIFORM1IPROC _glProgramUniform1i;
    public static void glProgramUniform1i(uint program, int location, int v0) => _glProgramUniform1i(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1IVPROC(uint program, int location, int count, in int value);
    private static PFNGLPROGRAMUNIFORM1IVPROC _glProgramUniform1iv;
    public static void glProgramUniform1iv(uint program, int location, int count, in int value) => _glProgramUniform1iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1FPROC(uint program, int location, float v0);
    private static PFNGLPROGRAMUNIFORM1FPROC _glProgramUniform1f;
    public static void glProgramUniform1f(uint program, int location, float v0) => _glProgramUniform1f(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1FVPROC(uint program, int location, int count, in float value);
    private static PFNGLPROGRAMUNIFORM1FVPROC _glProgramUniform1fv;
    public static void glProgramUniform1fv(uint program, int location, int count, in float value) => _glProgramUniform1fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1DPROC(uint program, int location, double v0);
    private static PFNGLPROGRAMUNIFORM1DPROC _glProgramUniform1d;
    public static void glProgramUniform1d(uint program, int location, double v0) => _glProgramUniform1d(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1DVPROC(uint program, int location, int count, in double value);
    private static PFNGLPROGRAMUNIFORM1DVPROC _glProgramUniform1dv;
    public static void glProgramUniform1dv(uint program, int location, int count, in double value) => _glProgramUniform1dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1UIPROC(uint program, int location, uint v0);
    private static PFNGLPROGRAMUNIFORM1UIPROC _glProgramUniform1ui;
    public static void glProgramUniform1ui(uint program, int location, uint v0) => _glProgramUniform1ui(program, location, v0);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM1UIVPROC(uint program, int location, int count, in uint value);
    private static PFNGLPROGRAMUNIFORM1UIVPROC _glProgramUniform1uiv;
    public static void glProgramUniform1uiv(uint program, int location, int count, in uint value) => _glProgramUniform1uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2IPROC(uint program, int location, int v0, int v1);
    private static PFNGLPROGRAMUNIFORM2IPROC _glProgramUniform2i;
    public static void glProgramUniform2i(uint program, int location, int v0, int v1) => _glProgramUniform2i(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2IVPROC(uint program, int location, int count, in int value);
    private static PFNGLPROGRAMUNIFORM2IVPROC _glProgramUniform2iv;
    public static void glProgramUniform2iv(uint program, int location, int count, in int value) => _glProgramUniform2iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2FPROC(uint program, int location, float v0, float v1);
    private static PFNGLPROGRAMUNIFORM2FPROC _glProgramUniform2f;
    public static void glProgramUniform2f(uint program, int location, float v0, float v1) => _glProgramUniform2f(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2FVPROC(uint program, int location, int count, in float value);
    private static PFNGLPROGRAMUNIFORM2FVPROC _glProgramUniform2fv;
    public static void glProgramUniform2fv(uint program, int location, int count, in float value) => _glProgramUniform2fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2DPROC(uint program, int location, double v0, double v1);
    private static PFNGLPROGRAMUNIFORM2DPROC _glProgramUniform2d;
    public static void glProgramUniform2d(uint program, int location, double v0, double v1) => _glProgramUniform2d(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2DVPROC(uint program, int location, int count, in double value);
    private static PFNGLPROGRAMUNIFORM2DVPROC _glProgramUniform2dv;
    public static void glProgramUniform2dv(uint program, int location, int count, in double value) => _glProgramUniform2dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2UIPROC(uint program, int location, uint v0, uint v1);
    private static PFNGLPROGRAMUNIFORM2UIPROC _glProgramUniform2ui;
    public static void glProgramUniform2ui(uint program, int location, uint v0, uint v1) => _glProgramUniform2ui(program, location, v0, v1);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM2UIVPROC(uint program, int location, int count, in uint value);
    private static PFNGLPROGRAMUNIFORM2UIVPROC _glProgramUniform2uiv;
    public static void glProgramUniform2uiv(uint program, int location, int count, in uint value) => _glProgramUniform2uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3IPROC(uint program, int location, int v0, int v1, int v2);
    private static PFNGLPROGRAMUNIFORM3IPROC _glProgramUniform3i;
    public static void glProgramUniform3i(uint program, int location, int v0, int v1, int v2) => _glProgramUniform3i(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3IVPROC(uint program, int location, int count, in int value);
    private static PFNGLPROGRAMUNIFORM3IVPROC _glProgramUniform3iv;
    public static void glProgramUniform3iv(uint program, int location, int count, in int value) => _glProgramUniform3iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3FPROC(uint program, int location, float v0, float v1, float v2);
    private static PFNGLPROGRAMUNIFORM3FPROC _glProgramUniform3f;
    public static void glProgramUniform3f(uint program, int location, float v0, float v1, float v2) => _glProgramUniform3f(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3FVPROC(uint program, int location, int count, in float value);
    private static PFNGLPROGRAMUNIFORM3FVPROC _glProgramUniform3fv;
    public static void glProgramUniform3fv(uint program, int location, int count, in float value) => _glProgramUniform3fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3DPROC(uint program, int location, double v0, double v1, double v2);
    private static PFNGLPROGRAMUNIFORM3DPROC _glProgramUniform3d;
    public static void glProgramUniform3d(uint program, int location, double v0, double v1, double v2) => _glProgramUniform3d(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3DVPROC(uint program, int location, int count, in double value);
    private static PFNGLPROGRAMUNIFORM3DVPROC _glProgramUniform3dv;
    public static void glProgramUniform3dv(uint program, int location, int count, in double value) => _glProgramUniform3dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3UIPROC(uint program, int location, uint v0, uint v1, uint v2);
    private static PFNGLPROGRAMUNIFORM3UIPROC _glProgramUniform3ui;
    public static void glProgramUniform3ui(uint program, int location, uint v0, uint v1, uint v2) => _glProgramUniform3ui(program, location, v0, v1, v2);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM3UIVPROC(uint program, int location, int count, in uint value);
    private static PFNGLPROGRAMUNIFORM3UIVPROC _glProgramUniform3uiv;
    public static void glProgramUniform3uiv(uint program, int location, int count, in uint value) => _glProgramUniform3uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4IPROC(uint program, int location, int v0, int v1, int v2, int v3);
    private static PFNGLPROGRAMUNIFORM4IPROC _glProgramUniform4i;
    public static void glProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3) => _glProgramUniform4i(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4IVPROC(uint program, int location, int count, in int value);
    private static PFNGLPROGRAMUNIFORM4IVPROC _glProgramUniform4iv;
    public static void glProgramUniform4iv(uint program, int location, int count, in int value) => _glProgramUniform4iv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4FPROC(uint program, int location, float v0, float v1, float v2, float v3);
    private static PFNGLPROGRAMUNIFORM4FPROC _glProgramUniform4f;
    public static void glProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3) => _glProgramUniform4f(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4FVPROC(uint program, int location, int count, in float value);
    private static PFNGLPROGRAMUNIFORM4FVPROC _glProgramUniform4fv;
    public static void glProgramUniform4fv(uint program, int location, int count, in float value) => _glProgramUniform4fv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4DPROC(uint program, int location, double v0, double v1, double v2, double v3);
    private static PFNGLPROGRAMUNIFORM4DPROC _glProgramUniform4d;
    public static void glProgramUniform4d(uint program, int location, double v0, double v1, double v2, double v3) => _glProgramUniform4d(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4DVPROC(uint program, int location, int count, in double value);
    private static PFNGLPROGRAMUNIFORM4DVPROC _glProgramUniform4dv;
    public static void glProgramUniform4dv(uint program, int location, int count, in double value) => _glProgramUniform4dv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4UIPROC(uint program, int location, uint v0, uint v1, uint v2, uint v3);
    private static PFNGLPROGRAMUNIFORM4UIPROC _glProgramUniform4ui;
    public static void glProgramUniform4ui(uint program, int location, uint v0, uint v1, uint v2, uint v3) => _glProgramUniform4ui(program, location, v0, v1, v2, v3);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORM4UIVPROC(uint program, int location, int count, in uint value);
    private static PFNGLPROGRAMUNIFORM4UIVPROC _glProgramUniform4uiv;
    public static void glProgramUniform4uiv(uint program, int location, int count, in uint value) => _glProgramUniform4uiv(program, location, count, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX2FVPROC _glProgramUniformMatrix2fv;
    public static void glProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix2fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX3FVPROC _glProgramUniformMatrix3fv;
    public static void glProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix3fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX4FVPROC _glProgramUniformMatrix4fv;
    public static void glProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix4fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX2DVPROC _glProgramUniformMatrix2dv;
    public static void glProgramUniformMatrix2dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix2dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX3DVPROC _glProgramUniformMatrix3dv;
    public static void glProgramUniformMatrix3dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix3dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX4DVPROC _glProgramUniformMatrix4dv;
    public static void glProgramUniformMatrix4dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix4dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC _glProgramUniformMatrix2x3fv;
    public static void glProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix2x3fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC _glProgramUniformMatrix3x2fv;
    public static void glProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix3x2fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC _glProgramUniformMatrix2x4fv;
    public static void glProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix2x4fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC _glProgramUniformMatrix4x2fv;
    public static void glProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix4x2fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC _glProgramUniformMatrix3x4fv;
    public static void glProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix3x4fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC(uint program, int location, int count, bool transpose, in float value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC _glProgramUniformMatrix4x3fv;
    public static void glProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, in float value) => _glProgramUniformMatrix4x3fv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC _glProgramUniformMatrix2x3dv;
    public static void glProgramUniformMatrix2x3dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix2x3dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC _glProgramUniformMatrix3x2dv;
    public static void glProgramUniformMatrix3x2dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix3x2dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC _glProgramUniformMatrix2x4dv;
    public static void glProgramUniformMatrix2x4dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix2x4dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC _glProgramUniformMatrix4x2dv;
    public static void glProgramUniformMatrix4x2dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix4x2dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC _glProgramUniformMatrix3x4dv;
    public static void glProgramUniformMatrix3x4dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix3x4dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC(uint program, int location, int count, bool transpose, in double value);
    private static PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC _glProgramUniformMatrix4x3dv;
    public static void glProgramUniformMatrix4x3dv(uint program, int location, int count, bool transpose, in double value) => _glProgramUniformMatrix4x3dv(program, location, count, transpose, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVALIDATEPROGRAMPIPELINEPROC(uint pipeline);
    private static PFNGLVALIDATEPROGRAMPIPELINEPROC _glValidateProgramPipeline;
    public static void glValidateProgramPipeline(uint pipeline) => _glValidateProgramPipeline(pipeline);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMPIPELINEINFOLOGPROC(uint pipeline, int bufSize, out int length, string infoLog);
    private static PFNGLGETPROGRAMPIPELINEINFOLOGPROC _glGetProgramPipelineInfoLog;
    public static void glGetProgramPipelineInfoLog(uint pipeline, int bufSize, out int length, string infoLog) => _glGetProgramPipelineInfoLog(pipeline, bufSize, out length, infoLog);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL1DPROC(uint index, double x);
    private static PFNGLVERTEXATTRIBL1DPROC _glVertexAttribL1d;
    public static void glVertexAttribL1d(uint index, double x) => _glVertexAttribL1d(index, x);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL2DPROC(uint index, double x, double y);
    private static PFNGLVERTEXATTRIBL2DPROC _glVertexAttribL2d;
    public static void glVertexAttribL2d(uint index, double x, double y) => _glVertexAttribL2d(index, x, y);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL3DPROC(uint index, double x, double y, double z);
    private static PFNGLVERTEXATTRIBL3DPROC _glVertexAttribL3d;
    public static void glVertexAttribL3d(uint index, double x, double y, double z) => _glVertexAttribL3d(index, x, y, z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL4DPROC(uint index, double x, double y, double z, double w);
    private static PFNGLVERTEXATTRIBL4DPROC _glVertexAttribL4d;
    public static void glVertexAttribL4d(uint index, double x, double y, double z, double w) => _glVertexAttribL4d(index, x, y, z, w);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL1DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIBL1DVPROC _glVertexAttribL1dv;
    public static void glVertexAttribL1dv(uint index, in double v) => _glVertexAttribL1dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL2DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIBL2DVPROC _glVertexAttribL2dv;
    public static void glVertexAttribL2dv(uint index, in double v) => _glVertexAttribL2dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL3DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIBL3DVPROC _glVertexAttribL3dv;
    public static void glVertexAttribL3dv(uint index, in double v) => _glVertexAttribL3dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBL4DVPROC(uint index, in double v);
    private static PFNGLVERTEXATTRIBL4DVPROC _glVertexAttribL4dv;
    public static void glVertexAttribL4dv(uint index, in double v) => _glVertexAttribL4dv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBLPOINTERPROC(uint index, int size, uint type, int stride, IntPtr pointer);
    private static PFNGLVERTEXATTRIBLPOINTERPROC _glVertexAttribLPointer;
    public static void glVertexAttribLPointer(uint index, int size, uint type, int stride, IntPtr pointer) => _glVertexAttribLPointer(index, size, type, stride, pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXATTRIBLDVPROC(uint index, uint pname, out double @params);
    private static PFNGLGETVERTEXATTRIBLDVPROC _glGetVertexAttribLdv;
    public static void glGetVertexAttribLdv(uint index, uint pname, out double @params) => _glGetVertexAttribLdv(index, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTARRAYVPROC(uint first, int count, in float v);
    private static PFNGLVIEWPORTARRAYVPROC _glViewportArrayv;
    public static void glViewportArrayv(uint first, int count, in float v) => _glViewportArrayv(first, count, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTINDEXEDFPROC(uint index, float x, float y, float w, float h);
    private static PFNGLVIEWPORTINDEXEDFPROC _glViewportIndexedf;
    public static void glViewportIndexedf(uint index, float x, float y, float w, float h) => _glViewportIndexedf(index, x, y, w, h);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVIEWPORTINDEXEDFVPROC(uint index, in float v);
    private static PFNGLVIEWPORTINDEXEDFVPROC _glViewportIndexedfv;
    public static void glViewportIndexedfv(uint index, in float v) => _glViewportIndexedfv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORARRAYVPROC(uint first, int count, in int v);
    private static PFNGLSCISSORARRAYVPROC _glScissorArrayv;
    public static void glScissorArrayv(uint first, int count, in int v) => _glScissorArrayv(first, count, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORINDEXEDPROC(uint index, int left, int bottom, int width, int height);
    private static PFNGLSCISSORINDEXEDPROC _glScissorIndexed;
    public static void glScissorIndexed(uint index, int left, int bottom, int width, int height) => _glScissorIndexed(index, left, bottom, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSCISSORINDEXEDVPROC(uint index, in int v);
    private static PFNGLSCISSORINDEXEDVPROC _glScissorIndexedv;
    public static void glScissorIndexedv(uint index, in int v) => _glScissorIndexedv(index, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEARRAYVPROC(uint first, int count, in double v);
    private static PFNGLDEPTHRANGEARRAYVPROC _glDepthRangeArrayv;
    public static void glDepthRangeArrayv(uint first, int count, in double v) => _glDepthRangeArrayv(first, count, in v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEPTHRANGEINDEXEDPROC(uint index, double n, double f);
    private static PFNGLDEPTHRANGEINDEXEDPROC _glDepthRangeIndexed;
    public static void glDepthRangeIndexed(uint index, double n, double f) => _glDepthRangeIndexed(index, n, f);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFLOATI_VPROC(uint target, uint index, out float data);
    private static PFNGLGETFLOATI_VPROC _glGetFloati_v;
    public static void glGetFloati_v(uint target, uint index, out float data) => _glGetFloati_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETDOUBLEI_VPROC(uint target, uint index, out double data);
    private static PFNGLGETDOUBLEI_VPROC _glGetDoublei_v;
    public static void glGetDoublei_v(uint target, uint index, out double data) => _glGetDoublei_v(target, index, out data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC(uint mode, int first, int count, int instancecount, uint baseinstance);
    private static PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC _glDrawArraysInstancedBaseInstance;
    public static void glDrawArraysInstancedBaseInstance(uint mode, int first, int count, int instancecount, uint baseinstance) => _glDrawArraysInstancedBaseInstance(mode, first, count, instancecount, baseinstance);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC(uint mode, int count, uint type, IntPtr indices, int instancecount, uint baseinstance);
    private static PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC _glDrawElementsInstancedBaseInstance;
    public static void glDrawElementsInstancedBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, uint baseinstance) => _glDrawElementsInstancedBaseInstance(mode, count, type, indices, instancecount, baseinstance);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex, uint baseinstance);
    private static PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC _glDrawElementsInstancedBaseVertexBaseInstance;
    public static void glDrawElementsInstancedBaseVertexBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex, uint baseinstance) => _glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices, instancecount, basevertex, baseinstance);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTERNALFORMATIVPROC(uint target, uint internalformat, uint pname, int count, out int @params);
    private static PFNGLGETINTERNALFORMATIVPROC _glGetInternalformativ;
    public static void glGetInternalformativ(uint target, uint internalformat, uint pname, int count, out int @params) => _glGetInternalformativ(target, internalformat, pname, count, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC(uint program, uint bufferIndex, uint pname, out int @params);
    private static PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC _glGetActiveAtomicCounterBufferiv;
    public static void glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, out int @params) => _glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDIMAGETEXTUREPROC(uint unit, uint texture, int level, bool layered, int layer, uint access, uint format);
    private static PFNGLBINDIMAGETEXTUREPROC _glBindImageTexture;
    public static void glBindImageTexture(uint unit, uint texture, int level, bool layered, int layer, uint access, uint format) => _glBindImageTexture(unit, texture, level, layered, layer, access, format);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMEMORYBARRIERPROC(uint barriers);
    private static PFNGLMEMORYBARRIERPROC _glMemoryBarrier;
    public static void glMemoryBarrier(uint barriers) => _glMemoryBarrier(barriers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE1DPROC(uint target, int levels, uint internalformat, int width);
    private static PFNGLTEXSTORAGE1DPROC _glTexStorage1D;
    public static void glTexStorage1D(uint target, int levels, uint internalformat, int width) => _glTexStorage1D(target, levels, internalformat, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE2DPROC(uint target, int levels, uint internalformat, int width, int height);
    private static PFNGLTEXSTORAGE2DPROC _glTexStorage2D;
    public static void glTexStorage2D(uint target, int levels, uint internalformat, int width, int height) => _glTexStorage2D(target, levels, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE3DPROC(uint target, int levels, uint internalformat, int width, int height, int depth);
    private static PFNGLTEXSTORAGE3DPROC _glTexStorage3D;
    public static void glTexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth) => _glTexStorage3D(target, levels, internalformat, width, height, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC(uint mode, uint id, int instancecount);
    private static PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC _glDrawTransformFeedbackInstanced;
    public static void glDrawTransformFeedbackInstanced(uint mode, uint id, int instancecount) => _glDrawTransformFeedbackInstanced(mode, id, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC(uint mode, uint id, uint stream, int instancecount);
    private static PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC _glDrawTransformFeedbackStreamInstanced;
    public static void glDrawTransformFeedbackStreamInstanced(uint mode, uint id, uint stream, int instancecount) => _glDrawTransformFeedbackStreamInstanced(mode, id, stream, instancecount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERDATAPROC(uint target, uint internalformat, uint format, uint type, IntPtr data);
    private static PFNGLCLEARBUFFERDATAPROC _glClearBufferData;
    public static void glClearBufferData(uint target, uint internalformat, uint format, uint type, IntPtr data) => _glClearBufferData(target, internalformat, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARBUFFERSUBDATAPROC(uint target, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data);
    private static PFNGLCLEARBUFFERSUBDATAPROC _glClearBufferSubData;
    public static void glClearBufferSubData(uint target, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data) => _glClearBufferSubData(target, internalformat, offset, size, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISPATCHCOMPUTEPROC(uint num_groups_x, uint num_groups_y, uint num_groups_z);
    private static PFNGLDISPATCHCOMPUTEPROC _glDispatchCompute;
    public static void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z) => _glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISPATCHCOMPUTEINDIRECTPROC(ulong indirect);
    private static PFNGLDISPATCHCOMPUTEINDIRECTPROC _glDispatchComputeIndirect;
    public static void glDispatchComputeIndirect(ulong indirect) => _glDispatchComputeIndirect(indirect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYIMAGESUBDATAPROC(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);
    private static PFNGLCOPYIMAGESUBDATAPROC _glCopyImageSubData;
    public static void glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) => _glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFRAMEBUFFERPARAMETERIPROC(uint target, uint pname, int param);
    private static PFNGLFRAMEBUFFERPARAMETERIPROC _glFramebufferParameteri;
    public static void glFramebufferParameteri(uint target, uint pname, int param) => _glFramebufferParameteri(target, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETFRAMEBUFFERPARAMETERIVPROC(uint target, uint pname, out int @params);
    private static PFNGLGETFRAMEBUFFERPARAMETERIVPROC _glGetFramebufferParameteriv;
    public static void glGetFramebufferParameteriv(uint target, uint pname, out int @params) => _glGetFramebufferParameteriv(target, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETINTERNALFORMATI64VPROC(uint target, uint internalformat, uint pname, int count, out long @params);
    private static PFNGLGETINTERNALFORMATI64VPROC _glGetInternalformati64v;
    public static void glGetInternalformati64v(uint target, uint internalformat, uint pname, int count, out long @params) => _glGetInternalformati64v(target, internalformat, pname, count, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATETEXSUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth);
    private static PFNGLINVALIDATETEXSUBIMAGEPROC _glInvalidateTexSubImage;
    public static void glInvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth) => _glInvalidateTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATETEXIMAGEPROC(uint texture, int level);
    private static PFNGLINVALIDATETEXIMAGEPROC _glInvalidateTexImage;
    public static void glInvalidateTexImage(uint texture, int level) => _glInvalidateTexImage(texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEBUFFERSUBDATAPROC(uint buffer, ulong offset, ulong length);
    private static PFNGLINVALIDATEBUFFERSUBDATAPROC _glInvalidateBufferSubData;
    public static void glInvalidateBufferSubData(uint buffer, ulong offset, ulong length) => _glInvalidateBufferSubData(buffer, offset, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEBUFFERDATAPROC(uint buffer);
    private static PFNGLINVALIDATEBUFFERDATAPROC _glInvalidateBufferData;
    public static void glInvalidateBufferData(uint buffer) => _glInvalidateBufferData(buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATEFRAMEBUFFERPROC(uint target, int numAttachments, in uint attachments);
    private static PFNGLINVALIDATEFRAMEBUFFERPROC _glInvalidateFramebuffer;
    public static void glInvalidateFramebuffer(uint target, int numAttachments, in uint attachments) => _glInvalidateFramebuffer(target, numAttachments, in attachments);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATESUBFRAMEBUFFERPROC(uint target, int numAttachments, in uint attachments, int x, int y, int width, int height);
    private static PFNGLINVALIDATESUBFRAMEBUFFERPROC _glInvalidateSubFramebuffer;
    public static void glInvalidateSubFramebuffer(uint target, int numAttachments, in uint attachments, int x, int y, int width, int height) => _glInvalidateSubFramebuffer(target, numAttachments, in attachments, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSINDIRECTPROC(uint mode, IntPtr indirect, int drawcount, int stride);
    private static PFNGLMULTIDRAWARRAYSINDIRECTPROC _glMultiDrawArraysIndirect;
    public static void glMultiDrawArraysIndirect(uint mode, IntPtr indirect, int drawcount, int stride) => _glMultiDrawArraysIndirect(mode, indirect, drawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSINDIRECTPROC(uint mode, uint type, IntPtr indirect, int drawcount, int stride);
    private static PFNGLMULTIDRAWELEMENTSINDIRECTPROC _glMultiDrawElementsIndirect;
    public static void glMultiDrawElementsIndirect(uint mode, uint type, IntPtr indirect, int drawcount, int stride) => _glMultiDrawElementsIndirect(mode, type, indirect, drawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMINTERFACEIVPROC(uint program, uint programInterface, uint pname, out int @params);
    private static PFNGLGETPROGRAMINTERFACEIVPROC _glGetProgramInterfaceiv;
    public static void glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, out int @params) => _glGetProgramInterfaceiv(program, programInterface, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETPROGRAMRESOURCEINDEXPROC(uint program, uint programInterface, string name);
    private static PFNGLGETPROGRAMRESOURCEINDEXPROC _glGetProgramResourceIndex;
    public static uint glGetProgramResourceIndex(uint program, uint programInterface, string name) => _glGetProgramResourceIndex(program, programInterface, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMRESOURCENAMEPROC(uint program, uint programInterface, uint index, int bufSize, out int length, string name);
    private static PFNGLGETPROGRAMRESOURCENAMEPROC _glGetProgramResourceName;
    public static void glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, out int length, string name) => _glGetProgramResourceName(program, programInterface, index, bufSize, out length, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPROGRAMRESOURCEIVPROC(uint program, uint programInterface, uint index, int propCount, in uint props, int count, out int length, out int @params);
    private static PFNGLGETPROGRAMRESOURCEIVPROC _glGetProgramResourceiv;
    public static void glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, in uint props, int count, out int length, out int @params) => _glGetProgramResourceiv(program, programInterface, index, propCount, in props, count, out length, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETPROGRAMRESOURCELOCATIONPROC(uint program, uint programInterface, string name);
    private static PFNGLGETPROGRAMRESOURCELOCATIONPROC _glGetProgramResourceLocation;
    public static int glGetProgramResourceLocation(uint program, uint programInterface, string name) => _glGetProgramResourceLocation(program, programInterface, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC(uint program, uint programInterface, string name);
    private static PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC _glGetProgramResourceLocationIndex;
    public static int glGetProgramResourceLocationIndex(uint program, uint programInterface, string name) => _glGetProgramResourceLocationIndex(program, programInterface, name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSHADERSTORAGEBLOCKBINDINGPROC(uint program, uint storageBlockIndex, uint storageBlockBinding);
    private static PFNGLSHADERSTORAGEBLOCKBINDINGPROC _glShaderStorageBlockBinding;
    public static void glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) => _glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXBUFFERRANGEPROC(uint target, uint internalformat, uint buffer, ulong offset, ulong size);
    private static PFNGLTEXBUFFERRANGEPROC _glTexBufferRange;
    public static void glTexBufferRange(uint target, uint internalformat, uint buffer, ulong offset, ulong size) => _glTexBufferRange(target, internalformat, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE2DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
    private static PFNGLTEXSTORAGE2DMULTISAMPLEPROC _glTexStorage2DMultisample;
    public static void glTexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations) => _glTexStorage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXSTORAGE3DMULTISAMPLEPROC(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
    private static PFNGLTEXSTORAGE3DMULTISAMPLEPROC _glTexStorage3DMultisample;
    public static void glTexStorage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTexStorage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREVIEWPROC(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);
    private static PFNGLTEXTUREVIEWPROC _glTextureView;
    public static void glTextureView(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers) => _glTextureView(texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXBUFFERPROC(uint bindingindex, uint buffer, ulong offset, int stride);
    private static PFNGLBINDVERTEXBUFFERPROC _glBindVertexBuffer;
    public static void glBindVertexBuffer(uint bindingindex, uint buffer, ulong offset, int stride) => _glBindVertexBuffer(bindingindex, buffer, offset, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBFORMATPROC(uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
    private static PFNGLVERTEXATTRIBFORMATPROC _glVertexAttribFormat;
    public static void glVertexAttribFormat(uint attribindex, int size, uint type, bool normalized, uint relativeoffset) => _glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBIFORMATPROC(uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXATTRIBIFORMATPROC _glVertexAttribIFormat;
    public static void glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset) => _glVertexAttribIFormat(attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBLFORMATPROC(uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXATTRIBLFORMATPROC _glVertexAttribLFormat;
    public static void glVertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset) => _glVertexAttribLFormat(attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXATTRIBBINDINGPROC(uint attribindex, uint bindingindex);
    private static PFNGLVERTEXATTRIBBINDINGPROC _glVertexAttribBinding;
    public static void glVertexAttribBinding(uint attribindex, uint bindingindex) => _glVertexAttribBinding(attribindex, bindingindex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXBINDINGDIVISORPROC(uint bindingindex, uint divisor);
    private static PFNGLVERTEXBINDINGDIVISORPROC _glVertexBindingDivisor;
    public static void glVertexBindingDivisor(uint bindingindex, uint divisor) => _glVertexBindingDivisor(bindingindex, divisor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGECONTROLPROC(uint source, uint type, uint severity, int count, in uint ids, bool enabled);
    private static PFNGLDEBUGMESSAGECONTROLPROC _glDebugMessageControl;
    public static void glDebugMessageControl(uint source, uint type, uint severity, int count, in uint ids, bool enabled) => _glDebugMessageControl(source, type, severity, count, in ids, enabled);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGEINSERTPROC(uint source, uint type, uint id, uint severity, int length, string buf);
    private static PFNGLDEBUGMESSAGEINSERTPROC _glDebugMessageInsert;
    public static void glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, string buf) => _glDebugMessageInsert(source, type, id, severity, length, buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDEBUGMESSAGECALLBACKPROC(GLDEBUGPROC callback, IntPtr userParam);
    private static PFNGLDEBUGMESSAGECALLBACKPROC _glDebugMessageCallback;
    public static void glDebugMessageCallback(GLDEBUGPROC callback, IntPtr userParam) => _glDebugMessageCallback(callback, userParam);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETDEBUGMESSAGELOGPROC(uint count, int bufSize, out uint sources, out uint types, out uint ids, out uint severities, out int lengths, string messageLog);
    private static PFNGLGETDEBUGMESSAGELOGPROC _glGetDebugMessageLog;
    public static uint glGetDebugMessageLog(uint count, int bufSize, out uint sources, out uint types, out uint ids, out uint severities, out int lengths, string messageLog) => _glGetDebugMessageLog(count, bufSize, out sources, out types, out ids, out severities, out lengths, messageLog);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPUSHDEBUGGROUPPROC(uint source, uint id, int length, string message);
    private static PFNGLPUSHDEBUGGROUPPROC _glPushDebugGroup;
    public static void glPushDebugGroup(uint source, uint id, int length, string message) => _glPushDebugGroup(source, id, length, message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOPDEBUGGROUPPROC();
    private static PFNGLPOPDEBUGGROUPPROC _glPopDebugGroup;
    public static void glPopDebugGroup() => _glPopDebugGroup();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLOBJECTLABELPROC(uint identifier, uint name, int length, string label);
    private static PFNGLOBJECTLABELPROC _glObjectLabel;
    public static void glObjectLabel(uint identifier, uint name, int length, string label) => _glObjectLabel(identifier, name, length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETOBJECTLABELPROC(uint identifier, uint name, int bufSize, out int length, string label);
    private static PFNGLGETOBJECTLABELPROC _glGetObjectLabel;
    public static void glGetObjectLabel(uint identifier, uint name, int bufSize, out int length, string label) => _glGetObjectLabel(identifier, name, bufSize, out length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLOBJECTPTRLABELPROC(IntPtr ptr, int length, string label);
    private static PFNGLOBJECTPTRLABELPROC _glObjectPtrLabel;
    public static void glObjectPtrLabel(IntPtr ptr, int length, string label) => _glObjectPtrLabel(ptr, length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETOBJECTPTRLABELPROC(IntPtr ptr, int bufSize, out int length, string label);
    private static PFNGLGETOBJECTPTRLABELPROC _glGetObjectPtrLabel;
    public static void glGetObjectPtrLabel(IntPtr ptr, int bufSize, out int length, string label) => _glGetObjectPtrLabel(ptr, bufSize, out length, label);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETPOINTERVPROC(uint pname, IntPtr @params);
    private static PFNGLGETPOINTERVPROC _glGetPointerv;
    public static void glGetPointerv(uint pname, IntPtr @params) => _glGetPointerv(pname, @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBUFFERSTORAGEPROC(uint target, ulong size, IntPtr data, uint flags);
    private static PFNGLBUFFERSTORAGEPROC _glBufferStorage;
    public static void glBufferStorage(uint target, ulong size, IntPtr data, uint flags) => _glBufferStorage(target, size, data, flags);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARTEXIMAGEPROC(uint texture, int level, uint format, uint type, IntPtr data);
    private static PFNGLCLEARTEXIMAGEPROC _glClearTexImage;
    public static void glClearTexImage(uint texture, int level, uint format, uint type, IntPtr data) => _glClearTexImage(texture, level, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARTEXSUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data);
    private static PFNGLCLEARTEXSUBIMAGEPROC _glClearTexSubImage;
    public static void glClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data) => _glClearTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERSBASEPROC(uint target, uint first, int count, in uint buffers);
    private static PFNGLBINDBUFFERSBASEPROC _glBindBuffersBase;
    public static void glBindBuffersBase(uint target, uint first, int count, in uint buffers) => _glBindBuffersBase(target, first, count, in buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDBUFFERSRANGEPROC(uint target, uint first, int count, in uint buffers, in ulong offsets, in ulong sizes);
    private static PFNGLBINDBUFFERSRANGEPROC _glBindBuffersRange;
    public static void glBindBuffersRange(uint target, uint first, int count, in uint buffers, in ulong offsets, in ulong sizes) => _glBindBuffersRange(target, first, count, in buffers, in offsets, in sizes);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTURESPROC(uint first, int count, in uint textures);
    private static PFNGLBINDTEXTURESPROC _glBindTextures;
    public static void glBindTextures(uint first, int count, in uint textures) => _glBindTextures(first, count, in textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDSAMPLERSPROC(uint first, int count, in uint samplers);
    private static PFNGLBINDSAMPLERSPROC _glBindSamplers;
    public static void glBindSamplers(uint first, int count, in uint samplers) => _glBindSamplers(first, count, in samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDIMAGETEXTURESPROC(uint first, int count, in uint textures);
    private static PFNGLBINDIMAGETEXTURESPROC _glBindImageTextures;
    public static void glBindImageTextures(uint first, int count, in uint textures) => _glBindImageTextures(first, count, in textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDVERTEXBUFFERSPROC(uint first, int count, in uint buffers, in ulong offsets, in int strides);
    private static PFNGLBINDVERTEXBUFFERSPROC _glBindVertexBuffers;
    public static void glBindVertexBuffers(uint first, int count, in uint buffers, in ulong offsets, in int strides) => _glBindVertexBuffers(first, count, in buffers, in offsets, in strides);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLIPCONTROLPROC(uint origin, uint depth);
    private static PFNGLCLIPCONTROLPROC _glClipControl;
    public static void glClipControl(uint origin, uint depth) => _glClipControl(origin, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATETRANSFORMFEEDBACKSPROC(int n, out uint ids);
    private static PFNGLCREATETRANSFORMFEEDBACKSPROC _glCreateTransformFeedbacks;
    public static void glCreateTransformFeedbacks(int n, out uint ids) => _glCreateTransformFeedbacks(n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC(uint xfb, uint index, uint buffer);
    private static PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC _glTransformFeedbackBufferBase;
    public static void glTransformFeedbackBufferBase(uint xfb, uint index, uint buffer) => _glTransformFeedbackBufferBase(xfb, index, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC(uint xfb, uint index, uint buffer, ulong offset, ulong size);
    private static PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC _glTransformFeedbackBufferRange;
    public static void glTransformFeedbackBufferRange(uint xfb, uint index, uint buffer, ulong offset, ulong size) => _glTransformFeedbackBufferRange(xfb, index, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKIVPROC(uint xfb, uint pname, out int param);
    private static PFNGLGETTRANSFORMFEEDBACKIVPROC _glGetTransformFeedbackiv;
    public static void glGetTransformFeedbackiv(uint xfb, uint pname, out int param) => _glGetTransformFeedbackiv(xfb, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKI_VPROC(uint xfb, uint pname, uint index, out int param);
    private static PFNGLGETTRANSFORMFEEDBACKI_VPROC _glGetTransformFeedbacki_v;
    public static void glGetTransformFeedbacki_v(uint xfb, uint pname, uint index, out int param) => _glGetTransformFeedbacki_v(xfb, pname, index, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTRANSFORMFEEDBACKI64_VPROC(uint xfb, uint pname, uint index, out long param);
    private static PFNGLGETTRANSFORMFEEDBACKI64_VPROC _glGetTransformFeedbacki64_v;
    public static void glGetTransformFeedbacki64_v(uint xfb, uint pname, uint index, out long param) => _glGetTransformFeedbacki64_v(xfb, pname, index, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEBUFFERSPROC(int n, out uint buffers);
    private static PFNGLCREATEBUFFERSPROC _glCreateBuffers;
    public static void glCreateBuffers(int n, out uint buffers) => _glCreateBuffers(n, out buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERSTORAGEPROC(uint buffer, ulong size, IntPtr data, uint flags);
    private static PFNGLNAMEDBUFFERSTORAGEPROC _glNamedBufferStorage;
    public static void glNamedBufferStorage(uint buffer, ulong size, IntPtr data, uint flags) => _glNamedBufferStorage(buffer, size, data, flags);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERDATAPROC(uint buffer, ulong size, IntPtr data, uint usage);
    private static PFNGLNAMEDBUFFERDATAPROC _glNamedBufferData;
    public static void glNamedBufferData(uint buffer, ulong size, IntPtr data, uint usage) => _glNamedBufferData(buffer, size, data, usage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDBUFFERSUBDATAPROC(uint buffer, ulong offset, ulong size, IntPtr data);
    private static PFNGLNAMEDBUFFERSUBDATAPROC _glNamedBufferSubData;
    public static void glNamedBufferSubData(uint buffer, ulong offset, ulong size, IntPtr data) => _glNamedBufferSubData(buffer, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYNAMEDBUFFERSUBDATAPROC(uint readBuffer, uint writeBuffer, ulong readOffset, ulong writeOffset, ulong size);
    private static PFNGLCOPYNAMEDBUFFERSUBDATAPROC _glCopyNamedBufferSubData;
    public static void glCopyNamedBufferSubData(uint readBuffer, uint writeBuffer, ulong readOffset, ulong writeOffset, ulong size) => _glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDBUFFERDATAPROC(uint buffer, uint internalformat, uint format, uint type, IntPtr data);
    private static PFNGLCLEARNAMEDBUFFERDATAPROC _glClearNamedBufferData;
    public static void glClearNamedBufferData(uint buffer, uint internalformat, uint format, uint type, IntPtr data) => _glClearNamedBufferData(buffer, internalformat, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDBUFFERSUBDATAPROC(uint buffer, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data);
    private static PFNGLCLEARNAMEDBUFFERSUBDATAPROC _glClearNamedBufferSubData;
    public static void glClearNamedBufferSubData(uint buffer, uint internalformat, ulong offset, ulong size, uint format, uint type, IntPtr data) => _glClearNamedBufferSubData(buffer, internalformat, offset, size, format, type, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPNAMEDBUFFERPROC(uint buffer, uint access);
    private static PFNGLMAPNAMEDBUFFERPROC _glMapNamedBuffer;
    public static IntPtr glMapNamedBuffer(uint buffer, uint access) => _glMapNamedBuffer(buffer, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr PFNGLMAPNAMEDBUFFERRANGEPROC(uint buffer, ulong offset, ulong length, uint access);
    private static PFNGLMAPNAMEDBUFFERRANGEPROC _glMapNamedBufferRange;
    public static IntPtr glMapNamedBufferRange(uint buffer, ulong offset, ulong length, uint access) => _glMapNamedBufferRange(buffer, offset, length, access);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate bool PFNGLUNMAPNAMEDBUFFERPROC(uint buffer);
    private static PFNGLUNMAPNAMEDBUFFERPROC _glUnmapNamedBuffer;
    public static bool glUnmapNamedBuffer(uint buffer) => _glUnmapNamedBuffer(buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC(uint buffer, ulong offset, ulong length);
    private static PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC _glFlushMappedNamedBufferRange;
    public static void glFlushMappedNamedBufferRange(uint buffer, ulong offset, ulong length) => _glFlushMappedNamedBufferRange(buffer, offset, length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPARAMETERIVPROC(uint buffer, uint pname, out int @params);
    private static PFNGLGETNAMEDBUFFERPARAMETERIVPROC _glGetNamedBufferParameteriv;
    public static void glGetNamedBufferParameteriv(uint buffer, uint pname, out int @params) => _glGetNamedBufferParameteriv(buffer, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPARAMETERI64VPROC(uint buffer, uint pname, out long @params);
    private static PFNGLGETNAMEDBUFFERPARAMETERI64VPROC _glGetNamedBufferParameteri64v;
    public static void glGetNamedBufferParameteri64v(uint buffer, uint pname, out long @params) => _glGetNamedBufferParameteri64v(buffer, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERPOINTERVPROC(uint buffer, uint pname, IntPtr @params);
    private static PFNGLGETNAMEDBUFFERPOINTERVPROC _glGetNamedBufferPointerv;
    public static void glGetNamedBufferPointerv(uint buffer, uint pname, IntPtr @params) => _glGetNamedBufferPointerv(buffer, pname, @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDBUFFERSUBDATAPROC(uint buffer, ulong offset, ulong size, IntPtr data);
    private static PFNGLGETNAMEDBUFFERSUBDATAPROC _glGetNamedBufferSubData;
    public static void glGetNamedBufferSubData(uint buffer, ulong offset, ulong size, IntPtr data) => _glGetNamedBufferSubData(buffer, offset, size, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEFRAMEBUFFERSPROC(int n, out uint framebuffers);
    private static PFNGLCREATEFRAMEBUFFERSPROC _glCreateFramebuffers;
    public static void glCreateFramebuffers(int n, out uint framebuffers) => _glCreateFramebuffers(n, out framebuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer);
    private static PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC _glNamedFramebufferRenderbuffer;
    public static void glNamedFramebufferRenderbuffer(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer) => _glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC(uint framebuffer, uint pname, int param);
    private static PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC _glNamedFramebufferParameteri;
    public static void glNamedFramebufferParameteri(uint framebuffer, uint pname, int param) => _glNamedFramebufferParameteri(framebuffer, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERTEXTUREPROC(uint framebuffer, uint attachment, uint texture, int level);
    private static PFNGLNAMEDFRAMEBUFFERTEXTUREPROC _glNamedFramebufferTexture;
    public static void glNamedFramebufferTexture(uint framebuffer, uint attachment, uint texture, int level) => _glNamedFramebufferTexture(framebuffer, attachment, texture, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC(uint framebuffer, uint attachment, uint texture, int level, int layer);
    private static PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC _glNamedFramebufferTextureLayer;
    public static void glNamedFramebufferTextureLayer(uint framebuffer, uint attachment, uint texture, int level, int layer) => _glNamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC(uint framebuffer, uint buf);
    private static PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC _glNamedFramebufferDrawBuffer;
    public static void glNamedFramebufferDrawBuffer(uint framebuffer, uint buf) => _glNamedFramebufferDrawBuffer(framebuffer, buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC(uint framebuffer, int n, in uint bufs);
    private static PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC _glNamedFramebufferDrawBuffers;
    public static void glNamedFramebufferDrawBuffers(uint framebuffer, int n, in uint bufs) => _glNamedFramebufferDrawBuffers(framebuffer, n, in bufs);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC(uint framebuffer, uint src);
    private static PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC _glNamedFramebufferReadBuffer;
    public static void glNamedFramebufferReadBuffer(uint framebuffer, uint src) => _glNamedFramebufferReadBuffer(framebuffer, src);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC(uint framebuffer, int numAttachments, in uint attachments);
    private static PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC _glInvalidateNamedFramebufferData;
    public static void glInvalidateNamedFramebufferData(uint framebuffer, int numAttachments, in uint attachments) => _glInvalidateNamedFramebufferData(framebuffer, numAttachments, in attachments);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC(uint framebuffer, int numAttachments, in uint attachments, int x, int y, int width, int height);
    private static PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC _glInvalidateNamedFramebufferSubData;
    public static void glInvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, in uint attachments, int x, int y, int width, int height) => _glInvalidateNamedFramebufferSubData(framebuffer, numAttachments, in attachments, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERIVPROC(uint framebuffer, uint buffer, int drawbuffer, in int value);
    private static PFNGLCLEARNAMEDFRAMEBUFFERIVPROC _glClearNamedFramebufferiv;
    public static void glClearNamedFramebufferiv(uint framebuffer, uint buffer, int drawbuffer, in int value) => _glClearNamedFramebufferiv(framebuffer, buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC(uint framebuffer, uint buffer, int drawbuffer, in uint value);
    private static PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC _glClearNamedFramebufferuiv;
    public static void glClearNamedFramebufferuiv(uint framebuffer, uint buffer, int drawbuffer, in uint value) => _glClearNamedFramebufferuiv(framebuffer, buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERFVPROC(uint framebuffer, uint buffer, int drawbuffer, in float value);
    private static PFNGLCLEARNAMEDFRAMEBUFFERFVPROC _glClearNamedFramebufferfv;
    public static void glClearNamedFramebufferfv(uint framebuffer, uint buffer, int drawbuffer, in float value) => _glClearNamedFramebufferfv(framebuffer, buffer, drawbuffer, in value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCLEARNAMEDFRAMEBUFFERFIPROC(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil);
    private static PFNGLCLEARNAMEDFRAMEBUFFERFIPROC _glClearNamedFramebufferfi;
    public static void glClearNamedFramebufferfi(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil) => _glClearNamedFramebufferfi(framebuffer, buffer, drawbuffer, depth, stencil);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBLITNAMEDFRAMEBUFFERPROC(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
    private static PFNGLBLITNAMEDFRAMEBUFFERPROC _glBlitNamedFramebuffer;
    public static void glBlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) => _glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC(uint framebuffer, uint target);
    private static PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC _glCheckNamedFramebufferStatus;
    public static uint glCheckNamedFramebufferStatus(uint framebuffer, uint target) => _glCheckNamedFramebufferStatus(framebuffer, target);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC(uint framebuffer, uint pname, out int param);
    private static PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC _glGetNamedFramebufferParameteriv;
    public static void glGetNamedFramebufferParameteriv(uint framebuffer, uint pname, out int param) => _glGetNamedFramebufferParameteriv(framebuffer, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC(uint framebuffer, uint attachment, uint pname, out int @params);
    private static PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC _glGetNamedFramebufferAttachmentParameteriv;
    public static void glGetNamedFramebufferAttachmentParameteriv(uint framebuffer, uint attachment, uint pname, out int @params) => _glGetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATERENDERBUFFERSPROC(int n, out uint renderbuffers);
    private static PFNGLCREATERENDERBUFFERSPROC _glCreateRenderbuffers;
    public static void glCreateRenderbuffers(int n, out uint renderbuffers) => _glCreateRenderbuffers(n, out renderbuffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDRENDERBUFFERSTORAGEPROC(uint renderbuffer, uint internalformat, int width, int height);
    private static PFNGLNAMEDRENDERBUFFERSTORAGEPROC _glNamedRenderbufferStorage;
    public static void glNamedRenderbufferStorage(uint renderbuffer, uint internalformat, int width, int height) => _glNamedRenderbufferStorage(renderbuffer, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC(uint renderbuffer, int samples, uint internalformat, int width, int height);
    private static PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC _glNamedRenderbufferStorageMultisample;
    public static void glNamedRenderbufferStorageMultisample(uint renderbuffer, int samples, uint internalformat, int width, int height) => _glNamedRenderbufferStorageMultisample(renderbuffer, samples, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC(uint renderbuffer, uint pname, out int @params);
    private static PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC _glGetNamedRenderbufferParameteriv;
    public static void glGetNamedRenderbufferParameteriv(uint renderbuffer, uint pname, out int @params) => _glGetNamedRenderbufferParameteriv(renderbuffer, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATETEXTURESPROC(uint target, int n, out uint textures);
    private static PFNGLCREATETEXTURESPROC _glCreateTextures;
    public static void glCreateTextures(uint target, int n, out uint textures) => _glCreateTextures(target, n, out textures);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBUFFERPROC(uint texture, uint internalformat, uint buffer);
    private static PFNGLTEXTUREBUFFERPROC _glTextureBuffer;
    public static void glTextureBuffer(uint texture, uint internalformat, uint buffer) => _glTextureBuffer(texture, internalformat, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBUFFERRANGEPROC(uint texture, uint internalformat, uint buffer, ulong offset, ulong size);
    private static PFNGLTEXTUREBUFFERRANGEPROC _glTextureBufferRange;
    public static void glTextureBufferRange(uint texture, uint internalformat, uint buffer, ulong offset, ulong size) => _glTextureBufferRange(texture, internalformat, buffer, offset, size);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE1DPROC(uint texture, int levels, uint internalformat, int width);
    private static PFNGLTEXTURESTORAGE1DPROC _glTextureStorage1D;
    public static void glTextureStorage1D(uint texture, int levels, uint internalformat, int width) => _glTextureStorage1D(texture, levels, internalformat, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE2DPROC(uint texture, int levels, uint internalformat, int width, int height);
    private static PFNGLTEXTURESTORAGE2DPROC _glTextureStorage2D;
    public static void glTextureStorage2D(uint texture, int levels, uint internalformat, int width, int height) => _glTextureStorage2D(texture, levels, internalformat, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE3DPROC(uint texture, int levels, uint internalformat, int width, int height, int depth);
    private static PFNGLTEXTURESTORAGE3DPROC _glTextureStorage3D;
    public static void glTextureStorage3D(uint texture, int levels, uint internalformat, int width, int height, int depth) => _glTextureStorage3D(texture, levels, internalformat, width, height, depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC(uint texture, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
    private static PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC _glTextureStorage2DMultisample;
    public static void glTextureStorage2DMultisample(uint texture, int samples, uint internalformat, int width, int height, bool fixedsamplelocations) => _glTextureStorage2DMultisample(texture, samples, internalformat, width, height, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC(uint texture, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
    private static PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC _glTextureStorage3DMultisample;
    public static void glTextureStorage3DMultisample(uint texture, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTextureStorage3DMultisample(texture, samples, internalformat, width, height, depth, fixedsamplelocations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXTURESUBIMAGE1DPROC _glTextureSubImage1D;
    public static void glTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, uint type, IntPtr pixels) => _glTextureSubImage1D(texture, level, xoffset, width, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXTURESUBIMAGE2DPROC _glTextureSubImage2D;
    public static void glTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels) => _glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
    private static PFNGLTEXTURESUBIMAGE3DPROC _glTextureSubImage3D;
    public static void glTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels) => _glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC _glCompressedTextureSubImage1D;
    public static void glCompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, int imageSize, IntPtr data) => _glCompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC _glCompressedTextureSubImage2D;
    public static void glCompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data) => _glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
    private static PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC _glCompressedTextureSubImage3D;
    public static void glCompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data) => _glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int x, int y, int width);
    private static PFNGLCOPYTEXTURESUBIMAGE1DPROC _glCopyTextureSubImage1D;
    public static void glCopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) => _glCopyTextureSubImage1D(texture, level, xoffset, x, y, width);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXTURESUBIMAGE2DPROC _glCopyTextureSubImage2D;
    public static void glCopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) => _glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCOPYTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
    private static PFNGLCOPYTEXTURESUBIMAGE3DPROC _glCopyTextureSubImage3D;
    public static void glCopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => _glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERFPROC(uint texture, uint pname, float param);
    private static PFNGLTEXTUREPARAMETERFPROC _glTextureParameterf;
    public static void glTextureParameterf(uint texture, uint pname, float param) => _glTextureParameterf(texture, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERFVPROC(uint texture, uint pname, in float param);
    private static PFNGLTEXTUREPARAMETERFVPROC _glTextureParameterfv;
    public static void glTextureParameterfv(uint texture, uint pname, in float param) => _glTextureParameterfv(texture, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIPROC(uint texture, uint pname, int param);
    private static PFNGLTEXTUREPARAMETERIPROC _glTextureParameteri;
    public static void glTextureParameteri(uint texture, uint pname, int param) => _glTextureParameteri(texture, pname, param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIIVPROC(uint texture, uint pname, in int @params);
    private static PFNGLTEXTUREPARAMETERIIVPROC _glTextureParameterIiv;
    public static void glTextureParameterIiv(uint texture, uint pname, in int @params) => _glTextureParameterIiv(texture, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIUIVPROC(uint texture, uint pname, in uint @params);
    private static PFNGLTEXTUREPARAMETERIUIVPROC _glTextureParameterIuiv;
    public static void glTextureParameterIuiv(uint texture, uint pname, in uint @params) => _glTextureParameterIuiv(texture, pname, in @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREPARAMETERIVPROC(uint texture, uint pname, in int param);
    private static PFNGLTEXTUREPARAMETERIVPROC _glTextureParameteriv;
    public static void glTextureParameteriv(uint texture, uint pname, in int param) => _glTextureParameteriv(texture, pname, in param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGENERATETEXTUREMIPMAPPROC(uint texture);
    private static PFNGLGENERATETEXTUREMIPMAPPROC _glGenerateTextureMipmap;
    public static void glGenerateTextureMipmap(uint texture) => _glGenerateTextureMipmap(texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLBINDTEXTUREUNITPROC(uint unit, uint texture);
    private static PFNGLBINDTEXTUREUNITPROC _glBindTextureUnit;
    public static void glBindTextureUnit(uint unit, uint texture) => _glBindTextureUnit(unit, texture);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREIMAGEPROC(uint texture, int level, uint format, uint type, int bufSize, IntPtr pixels);
    private static PFNGLGETTEXTUREIMAGEPROC _glGetTextureImage;
    public static void glGetTextureImage(uint texture, int level, uint format, uint type, int bufSize, IntPtr pixels) => _glGetTextureImage(texture, level, format, type, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC(uint texture, int level, int bufSize, IntPtr pixels);
    private static PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC _glGetCompressedTextureImage;
    public static void glGetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels) => _glGetCompressedTextureImage(texture, level, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURELEVELPARAMETERFVPROC(uint texture, int level, uint pname, out float @params);
    private static PFNGLGETTEXTURELEVELPARAMETERFVPROC _glGetTextureLevelParameterfv;
    public static void glGetTextureLevelParameterfv(uint texture, int level, uint pname, out float @params) => _glGetTextureLevelParameterfv(texture, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURELEVELPARAMETERIVPROC(uint texture, int level, uint pname, out int @params);
    private static PFNGLGETTEXTURELEVELPARAMETERIVPROC _glGetTextureLevelParameteriv;
    public static void glGetTextureLevelParameteriv(uint texture, int level, uint pname, out int @params) => _glGetTextureLevelParameteriv(texture, level, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERFVPROC(uint texture, uint pname, out float @params);
    private static PFNGLGETTEXTUREPARAMETERFVPROC _glGetTextureParameterfv;
    public static void glGetTextureParameterfv(uint texture, uint pname, out float @params) => _glGetTextureParameterfv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIIVPROC(uint texture, uint pname, out int @params);
    private static PFNGLGETTEXTUREPARAMETERIIVPROC _glGetTextureParameterIiv;
    public static void glGetTextureParameterIiv(uint texture, uint pname, out int @params) => _glGetTextureParameterIiv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIUIVPROC(uint texture, uint pname, out uint @params);
    private static PFNGLGETTEXTUREPARAMETERIUIVPROC _glGetTextureParameterIuiv;
    public static void glGetTextureParameterIuiv(uint texture, uint pname, out uint @params) => _glGetTextureParameterIuiv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTUREPARAMETERIVPROC(uint texture, uint pname, out int @params);
    private static PFNGLGETTEXTUREPARAMETERIVPROC _glGetTextureParameteriv;
    public static void glGetTextureParameteriv(uint texture, uint pname, out int @params) => _glGetTextureParameteriv(texture, pname, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEVERTEXARRAYSPROC(int n, out uint arrays);
    private static PFNGLCREATEVERTEXARRAYSPROC _glCreateVertexArrays;
    public static void glCreateVertexArrays(int n, out uint arrays) => _glCreateVertexArrays(n, out arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLDISABLEVERTEXARRAYATTRIBPROC(uint vaobj, uint index);
    private static PFNGLDISABLEVERTEXARRAYATTRIBPROC _glDisableVertexArrayAttrib;
    public static void glDisableVertexArrayAttrib(uint vaobj, uint index) => _glDisableVertexArrayAttrib(vaobj, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLENABLEVERTEXARRAYATTRIBPROC(uint vaobj, uint index);
    private static PFNGLENABLEVERTEXARRAYATTRIBPROC _glEnableVertexArrayAttrib;
    public static void glEnableVertexArrayAttrib(uint vaobj, uint index) => _glEnableVertexArrayAttrib(vaobj, index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYELEMENTBUFFERPROC(uint vaobj, uint buffer);
    private static PFNGLVERTEXARRAYELEMENTBUFFERPROC _glVertexArrayElementBuffer;
    public static void glVertexArrayElementBuffer(uint vaobj, uint buffer) => _glVertexArrayElementBuffer(vaobj, buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYVERTEXBUFFERPROC(uint vaobj, uint bindingindex, uint buffer, ulong offset, int stride);
    private static PFNGLVERTEXARRAYVERTEXBUFFERPROC _glVertexArrayVertexBuffer;
    public static void glVertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, ulong offset, int stride) => _glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYVERTEXBUFFERSPROC(uint vaobj, uint first, int count, in uint buffers, in ulong offsets, in int strides);
    private static PFNGLVERTEXARRAYVERTEXBUFFERSPROC _glVertexArrayVertexBuffers;
    public static void glVertexArrayVertexBuffers(uint vaobj, uint first, int count, in uint buffers, in ulong offsets, in int strides) => _glVertexArrayVertexBuffers(vaobj, first, count, in buffers, in offsets, in strides);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBBINDINGPROC(uint vaobj, uint attribindex, uint bindingindex);
    private static PFNGLVERTEXARRAYATTRIBBINDINGPROC _glVertexArrayAttribBinding;
    public static void glVertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex) => _glVertexArrayAttribBinding(vaobj, attribindex, bindingindex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBFORMATPROC(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
    private static PFNGLVERTEXARRAYATTRIBFORMATPROC _glVertexArrayAttribFormat;
    public static void glVertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset) => _glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBIFORMATPROC(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXARRAYATTRIBIFORMATPROC _glVertexArrayAttribIFormat;
    public static void glVertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => _glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYATTRIBLFORMATPROC(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
    private static PFNGLVERTEXARRAYATTRIBLFORMATPROC _glVertexArrayAttribLFormat;
    public static void glVertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => _glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativeoffset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLVERTEXARRAYBINDINGDIVISORPROC(uint vaobj, uint bindingindex, uint divisor);
    private static PFNGLVERTEXARRAYBINDINGDIVISORPROC _glVertexArrayBindingDivisor;
    public static void glVertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor) => _glVertexArrayBindingDivisor(vaobj, bindingindex, divisor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYIVPROC(uint vaobj, uint pname, out int param);
    private static PFNGLGETVERTEXARRAYIVPROC _glGetVertexArrayiv;
    public static void glGetVertexArrayiv(uint vaobj, uint pname, out int param) => _glGetVertexArrayiv(vaobj, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYINDEXEDIVPROC(uint vaobj, uint index, uint pname, out int param);
    private static PFNGLGETVERTEXARRAYINDEXEDIVPROC _glGetVertexArrayIndexediv;
    public static void glGetVertexArrayIndexediv(uint vaobj, uint index, uint pname, out int param) => _glGetVertexArrayIndexediv(vaobj, index, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETVERTEXARRAYINDEXED64IVPROC(uint vaobj, uint index, uint pname, out long param);
    private static PFNGLGETVERTEXARRAYINDEXED64IVPROC _glGetVertexArrayIndexed64iv;
    public static void glGetVertexArrayIndexed64iv(uint vaobj, uint index, uint pname, out long param) => _glGetVertexArrayIndexed64iv(vaobj, index, pname, out param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATESAMPLERSPROC(int n, out uint samplers);
    private static PFNGLCREATESAMPLERSPROC _glCreateSamplers;
    public static void glCreateSamplers(int n, out uint samplers) => _glCreateSamplers(n, out samplers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEPROGRAMPIPELINESPROC(int n, out uint pipelines);
    private static PFNGLCREATEPROGRAMPIPELINESPROC _glCreateProgramPipelines;
    public static void glCreateProgramPipelines(int n, out uint pipelines) => _glCreateProgramPipelines(n, out pipelines);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLCREATEQUERIESPROC(uint target, int n, out uint ids);
    private static PFNGLCREATEQUERIESPROC _glCreateQueries;
    public static void glCreateQueries(uint target, int n, out uint ids) => _glCreateQueries(target, n, out ids);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTI64VPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTI64VPROC _glGetQueryBufferObjecti64v;
    public static void glGetQueryBufferObjecti64v(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjecti64v(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTIVPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTIVPROC _glGetQueryBufferObjectiv;
    public static void glGetQueryBufferObjectiv(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjectiv(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTUI64VPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTUI64VPROC _glGetQueryBufferObjectui64v;
    public static void glGetQueryBufferObjectui64v(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjectui64v(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETQUERYBUFFEROBJECTUIVPROC(uint id, uint buffer, uint pname, ulong offset);
    private static PFNGLGETQUERYBUFFEROBJECTUIVPROC _glGetQueryBufferObjectuiv;
    public static void glGetQueryBufferObjectuiv(uint id, uint buffer, uint pname, ulong offset) => _glGetQueryBufferObjectuiv(id, buffer, pname, offset);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMEMORYBARRIERBYREGIONPROC(uint barriers);
    private static PFNGLMEMORYBARRIERBYREGIONPROC _glMemoryBarrierByRegion;
    public static void glMemoryBarrierByRegion(uint barriers) => _glMemoryBarrierByRegion(barriers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETTEXTURESUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, IntPtr pixels);
    private static PFNGLGETTEXTURESUBIMAGEPROC _glGetTextureSubImage;
    public static void glGetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, IntPtr pixels) => _glGetTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels);
    private static PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC _glGetCompressedTextureSubImage;
    public static void glGetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels) => _glGetCompressedTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFNGLGETGRAPHICSRESETSTATUSPROC();
    private static PFNGLGETGRAPHICSRESETSTATUSPROC _glGetGraphicsResetStatus;
    public static uint glGetGraphicsResetStatus() => _glGetGraphicsResetStatus();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNCOMPRESSEDTEXIMAGEPROC(uint target, int lod, int bufSize, IntPtr pixels);
    private static PFNGLGETNCOMPRESSEDTEXIMAGEPROC _glGetnCompressedTexImage;
    public static void glGetnCompressedTexImage(uint target, int lod, int bufSize, IntPtr pixels) => _glGetnCompressedTexImage(target, lod, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNTEXIMAGEPROC(uint target, int level, uint format, uint type, int bufSize, IntPtr pixels);
    private static PFNGLGETNTEXIMAGEPROC _glGetnTexImage;
    public static void glGetnTexImage(uint target, int level, uint format, uint type, int bufSize, IntPtr pixels) => _glGetnTexImage(target, level, format, type, bufSize, pixels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMDVPROC(uint program, int location, int bufSize, out double @params);
    private static PFNGLGETNUNIFORMDVPROC _glGetnUniformdv;
    public static void glGetnUniformdv(uint program, int location, int bufSize, out double @params) => _glGetnUniformdv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMFVPROC(uint program, int location, int bufSize, out float @params);
    private static PFNGLGETNUNIFORMFVPROC _glGetnUniformfv;
    public static void glGetnUniformfv(uint program, int location, int bufSize, out float @params) => _glGetnUniformfv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMIVPROC(uint program, int location, int bufSize, out int @params);
    private static PFNGLGETNUNIFORMIVPROC _glGetnUniformiv;
    public static void glGetnUniformiv(uint program, int location, int bufSize, out int @params) => _glGetnUniformiv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNUNIFORMUIVPROC(uint program, int location, int bufSize, out uint @params);
    private static PFNGLGETNUNIFORMUIVPROC _glGetnUniformuiv;
    public static void glGetnUniformuiv(uint program, int location, int bufSize, out uint @params) => _glGetnUniformuiv(program, location, bufSize, out @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLREADNPIXELSPROC(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data);
    private static PFNGLREADNPIXELSPROC _glReadnPixels;
    public static void glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data) => _glReadnPixels(x, y, width, height, format, type, bufSize, data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMAPDVPROC(uint target, uint query, int bufSize, out double v);
    private static PFNGLGETNMAPDVPROC _glGetnMapdv;
    public static void glGetnMapdv(uint target, uint query, int bufSize, out double v) => _glGetnMapdv(target, query, bufSize, out v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMAPFVPROC(uint target, uint query, int bufSize, out float v);
    private static PFNGLGETNMAPFVPROC _glGetnMapfv;
    public static void glGetnMapfv(uint target, uint query, int bufSize, out float v) => _glGetnMapfv(target, query, bufSize, out v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMAPIVPROC(uint target, uint query, int bufSize, out int v);
    private static PFNGLGETNMAPIVPROC _glGetnMapiv;
    public static void glGetnMapiv(uint target, uint query, int bufSize, out int v) => _glGetnMapiv(target, query, bufSize, out v);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPIXELMAPFVPROC(uint map, int bufSize, out float values);
    private static PFNGLGETNPIXELMAPFVPROC _glGetnPixelMapfv;
    public static void glGetnPixelMapfv(uint map, int bufSize, out float values) => _glGetnPixelMapfv(map, bufSize, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPIXELMAPUIVPROC(uint map, int bufSize, out uint values);
    private static PFNGLGETNPIXELMAPUIVPROC _glGetnPixelMapuiv;
    public static void glGetnPixelMapuiv(uint map, int bufSize, out uint values) => _glGetnPixelMapuiv(map, bufSize, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPIXELMAPUSVPROC(uint map, int bufSize, out ushort values);
    private static PFNGLGETNPIXELMAPUSVPROC _glGetnPixelMapusv;
    public static void glGetnPixelMapusv(uint map, int bufSize, out ushort values) => _glGetnPixelMapusv(map, bufSize, out values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNPOLYGONSTIPPLEPROC(int bufSize, IntPtr pattern);
    private static PFNGLGETNPOLYGONSTIPPLEPROC _glGetnPolygonStipple;
    public static void glGetnPolygonStipple(int bufSize, IntPtr pattern) => _glGetnPolygonStipple(bufSize, pattern);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNCOLORTABLEPROC(uint target, uint format, uint type, int bufSize, IntPtr table);
    private static PFNGLGETNCOLORTABLEPROC _glGetnColorTable;
    public static void glGetnColorTable(uint target, uint format, uint type, int bufSize, IntPtr table) => _glGetnColorTable(target, format, type, bufSize, table);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNCONVOLUTIONFILTERPROC(uint target, uint format, uint type, int bufSize, IntPtr image);
    private static PFNGLGETNCONVOLUTIONFILTERPROC _glGetnConvolutionFilter;
    public static void glGetnConvolutionFilter(uint target, uint format, uint type, int bufSize, IntPtr image) => _glGetnConvolutionFilter(target, format, type, bufSize, image);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNSEPARABLEFILTERPROC(uint target, uint format, uint type, int rowBufSize, IntPtr row, int columnBufSize, IntPtr column, IntPtr span);
    private static PFNGLGETNSEPARABLEFILTERPROC _glGetnSeparableFilter;
    public static void glGetnSeparableFilter(uint target, uint format, uint type, int rowBufSize, IntPtr row, int columnBufSize, IntPtr column, IntPtr span) => _glGetnSeparableFilter(target, format, type, rowBufSize, row, columnBufSize, column, span);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNHISTOGRAMPROC(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values);
    private static PFNGLGETNHISTOGRAMPROC _glGetnHistogram;
    public static void glGetnHistogram(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values) => _glGetnHistogram(target, reset, format, type, bufSize, values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLGETNMINMAXPROC(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values);
    private static PFNGLGETNMINMAXPROC _glGetnMinmax;
    public static void glGetnMinmax(uint target, bool reset, uint format, uint type, int bufSize, IntPtr values) => _glGetnMinmax(target, reset, format, type, bufSize, values);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLTEXTUREBARRIERPROC();
    private static PFNGLTEXTUREBARRIERPROC _glTextureBarrier;
    public static void glTextureBarrier() => _glTextureBarrier();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLSPECIALIZESHADERPROC(uint shader, string pEntryPoint, uint numSpecializationConstants, in uint pConstantIndex, in uint pConstantValue);
    private static PFNGLSPECIALIZESHADERPROC _glSpecializeShader;
    public static void glSpecializeShader(uint shader, string pEntryPoint, uint numSpecializationConstants, in uint pConstantIndex, in uint pConstantValue) => _glSpecializeShader(shader, pEntryPoint, numSpecializationConstants, in pConstantIndex, in pConstantValue);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC(uint mode, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride);
    private static PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC _glMultiDrawArraysIndirectCount;
    public static void glMultiDrawArraysIndirectCount(uint mode, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride) => _glMultiDrawArraysIndirectCount(mode, indirect, drawcount, maxdrawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC(uint mode, uint type, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride);
    private static PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC _glMultiDrawElementsIndirectCount;
    public static void glMultiDrawElementsIndirectCount(uint mode, uint type, IntPtr indirect, ulong drawcount, int maxdrawcount, int stride) => _glMultiDrawElementsIndirectCount(mode, type, indirect, drawcount, maxdrawcount, stride);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFNGLPOLYGONOFFSETCLAMPPROC(float factor, float units, float clamp);
    private static PFNGLPOLYGONOFFSETCLAMPPROC _glPolygonOffsetClamp;
    public static void glPolygonOffsetClamp(float factor, float units, float clamp) => _glPolygonOffsetClamp(factor, units, clamp);
}