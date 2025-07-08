using BSG.BackEnd.Services;
using BSG.Database;
using BSG.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSG.Helper;

internal static class Program
{
    private static BsgDbContext _db = null!;
    
    private static void Main()
    {
        var opts = new DbContextOptionsBuilder<BsgDbContext>();
        opts.UseSqlite("Data Source=C:\\Users\\juarias\\RiderProjects\\POC\\BSG\\BSG.Api\\Bsg.db;Cache=Shared");
        
        _db = new BsgDbContext( opts.Options, new DateConverterService() );
        
        SeedProductType();
    }

    private static void SeedProductType()
    {
        var types = new List<ProductType>
        {
            new()
            {
                Name = "Apparel",
                Products =
                [
                    new Product { Name = "Shirt", Code = "001", Description = "Shirt description" },
                    new Product { Name = "Pants", Code = "002", Description = "Pants description" },
                    new Product { Name = "Hats", Code = "003", Description = "Hats description" },
                    new Product { Name = "Shoes", Code = "004", Description = "Shoes description" },
                    new Product { Name = "Jackets", Code = "005", Description = "Jackets description" },
                    new Product { Name = "Socks", Code = "006", Description = "Socks description" },
                    new Product { Name = "Sweaters", Code = "007", Description = "Sweaters description" },
                    new Product { Name = "T-Shirts", Code = "008", Description = "T-Shirts description" },
                    new Product { Name = "Jeans", Code = "009", Description = "Jeans description" }
                ]
            },
            new()
            {
                Name = "Hardware",
                Products =
                [
                    new Product { Name = "HW Hammer", Code = "010", Description = "Hammer description" },
                    new Product { Name = "HW Screwdriver", Code = "011", Description = "Screwdriver description" },
                    new Product { Name = "HW Wrench", Code = "012", Description = "Wrench description" },
                    new Product { Name = "HW Saw", Code = "013", Description = "Saw description" },
                    new Product { Name = "HW Drill", Code = "014", Description = "Drill description" },
                    new Product { Name = "HW Crowbar", Code = "015", Description = "Crowbar description" },
                    new Product { Name = "HW Welder", Code = "016", Description = "Welder description" },
                    new Product { Name = "HW Soldering Iron", Code = "017", Description = "Soldering Iron description" },
                    new Product { Name = "HW Shovel", Code = "018", Description = "Shovel description" }
                ]
            },
            new()
            {
                Name = "Toys",
                Products =
                [
                    new Product { Name = "Toy Car", Code = "019", Description = "Toy Car description" },
                    new Product { Name = "Toy Ball", Code = "020", Description = "Toy Ball description" },
                    new Product { Name = "Toy Bird", Code = "021", Description = "Toy Bird description" },
                    new Product { Name = "Toy Cat", Code = "022", Description = "Toy Cat description" },
                    new Product { Name = "Toy Dog", Code = "023", Description = "Toy Dog description" },
                    new Product { Name = "Toy Horse", Code = "024", Description = "Toy Horse description" },
                    new Product { Name = "Toy Mouse", Code = "025", Description = "Toy Mouse description" },
                    new Product { Name = "Toy Rabbit", Code = "026", Description = "Toy Rabbit description" },
                    new Product { Name = "Toy Snake", Code = "027", Description = "Toy Snake description" },
                    new Product { Name = "Toy Spider", Code = "028", Description = "Toy Spider description" }
                ]
            },
            new()
            {
                Name = "Electronics",
                Products =
                [
                    new Product { Name = "Laptop", Code = "029", Description = "Laptop description" },
                    new Product { Name = "Phone", Code = "030", Description = "Phone description" },
                    new Product { Name = "Tablet", Code = "031", Description = "Tablet description" },
                    new Product { Name = "Computer", Code = "032", Description = "Computer description" },
                    new Product { Name = "Mouse", Code = "033", Description = "Mouse description" },
                    new Product { Name = "Keyboard", Code = "034", Description = "Keyboard description" },
                    new Product { Name = "Headphones", Code = "035", Description = "Headphones description" },
                    new Product { Name = "Camera", Code = "036", Description = "Camera description" },
                    new Product { Name = "Speaker", Code = "037", Description = "Speaker description" },
                    new Product { Name = "Monitor", Code = "038", Description = "Monitor description" },
                    new Product { Name = "Headset", Code = "039", Description = "Headset description" },
                    new Product { Name = "Power Bank", Code = "040", Description = "Power Bank description" },
                    new Product { Name = "Charger", Code = "041", Description = "Charger description" }
                ]
            },
            new()
            {
                Name = "House Appliances",
                Products =
                [
                    new Product { Name = "Microwave", Code = "042", Description = "Microwave description" },
                    new Product { Name = "Oven", Code = "043", Description = "Oven description" },
                    new Product { Name = "Toaster", Code = "044", Description = "Toaster description" },
                    new Product { Name = "Fridge", Code = "045", Description = "Fridge description" },
                    new Product { Name = "Washing Machine", Code = "046", Description = "Washing Machine description" },
                    new Product { Name = "Dishwasher", Code = "047", Description = "Dishwasher description" },
                    new Product { Name = "Blender", Code = "048", Description = "Blender description" },
                    new Product { Name = "Kettle", Code = "049", Description = "Kettle description" },
                    new Product { Name = "Microwave Oven", Code = "050", Description = "Microwave Oven description" },
                    new Product { Name = "Stove", Code = "051", Description = "Stove description" },
                    new Product { Name = "Furnace", Code = "052", Description = "Furnace description" }
                ]
            },
            new()
            {
                Name = "Linen",
                Products =
                [
                    new Product { Name = "Linen", Code = "053", Description = "Linen description" },
                    new Product { Name = "Cotton", Code = "054", Description = "Cotton description" },
                    new Product { Name = "Polyester", Code = "055", Description = "Polyester description" },
                    new Product { Name = "Wool", Code = "056", Description = "Wool description" },
                ]
            },
            new()
            {
                Name = "Car Parts",
                Products =
                [
                    new Product { Name = "Engine", Code = "057", Description = "Engine description" },
                    new Product { Name = "Transmission", Code = "058", Description = "Transmission description" },
                    new Product { Name = "Brakes", Code = "059", Description = "Brakes description" },
                    new Product { Name = "Wheels", Code = "060", Description = "Wheels description" },
                    new Product { Name = "Suspension", Code = "061", Description = "Suspension description" },
                    new Product { Name = "Tires", Code = "062", Description = "Tires description" }
                ]
            },
            new()
            {
                Name = "Kitchen tools",
                Products =
                [
                    new Product { Name = "Kitchen Knife", Code = "063", Description = "Knife description" },
                    new Product { Name = "Fork", Code = "064", Description = "Fork description" },
                    new Product { Name = "Spoon", Code = "065", Description = "Spoon description" },
                    new Product { Name = "Bowl", Code = "066", Description = "Bowl description" },
                    new Product { Name = "Plate", Code = "067", Description = "Plate description" },
                    new Product { Name = "Cutting Board", Code = "068", Description = "Cutting Board description" },
                    new Product { Name = "Mixing Bowl", Code = "069", Description = "Mixing Bowl description" },
                    new Product { Name = "Pan", Code = "070", Description = "Pan description" },
                    new Product { Name = "Spatula", Code = "071", Description = "Spatula description" }
                ]
            },
            new()
            {
                Name = "Cleaning Products",
                Products =
                [
                    new Product { Name = "Soap", Code = "072", Description = "Soap description" },
                    new Product { Name = "Shampoo", Code = "073", Description = "Shampoo description" },
                    new Product { Name = "Deodorant", Code = "074", Description = "Deodorant description" },
                    new Product { Name = "Toothpaste", Code = "075", Description = "Toothpaste description" },
                    new Product { Name = "Toothbrush", Code = "076", Description = "Toothbrush description" },
                    new Product { Name = "Mosquito Repellent", Code = "077", Description = "Mosquito Repellent description" },
                    new Product { Name = "Hand Sanitizer", Code = "078", Description = "Hand Sanitizer description" },
                    new Product { Name = "Tissue Box", Code = "079", Description = "Tissue Box description" },
                    new Product { Name = "Tissue Tube", Code = "080", Description = "Tissue Tube description" },
                    new Product { Name = "Tissue Paper", Code = "081", Description = "Tissue Paper description" },
                    new Product { Name = "Tissue Shampoo", Code = "082", Description = "Tissue Shampoo description" },
                    new Product { Name = "Tissue Soap", Code = "083", Description = "Tissue Soap description" },
                    new Product { Name = "Tissue Deodorant", Code = "084", Description = "Tissue Deodorant description" },
                    new Product { Name = "Trash bags", Code = "085", Description = "Trash bags Toothpaste description" },
                ]
            },
            new()
            {
                Name = "Software",
                Products =
                [
                    new Product { Name = "Windows", Code = "086", Description = "Windows description" },
                    new Product { Name = "Mac", Code = "087", Description = "Mac description" },
                    new Product { Name = "Linux", Code = "088", Description = "Linux description" },
                    new Product { Name = "Chrome", Code = "089", Description = "Chrome description" },
                    new Product { Name = "Firefox", Code = "090", Description = "Firefox description" },
                    new Product { Name = "Edge", Code = "091", Description = "Edge description" },
                    new Product { Name = "Safari", Code = "092", Description = "Safari description" },
                    new Product { Name = "Internet Explorer", Code = "093", Description = "Internet Explorer description" },
                    new Product { Name = "Opera", Code = "094", Description = "Opera description" },
                    new Product { Name = "Chrome OS", Code = "095", Description = "Chrome OS description" },
                    new Product { Name = "Android", Code = "096", Description = "Android description" },
                    new Product { Name = "Word", Code = "097", Description = "Word description" },
                    new Product { Name = "Excel", Code = "098", Description = "Excel description" },
                    new Product { Name = "PowerPoint", Code = "099", Description = "PowerPoint description" },
                    new Product { Name = "Outlook", Code = "100", Description = "Outlook description" },
                    new Product { Name = "OneNote", Code = "101", Description = "OneNote description" },
                    new Product { Name = "Access", Code = "102", Description = "Access description" },
                    new Product { Name = "Publisher", Code = "103", Description = "Publisher description" },
                    new Product { Name = "Visio", Code = "104", Description = "Visio description" },
                    new Product { Name = "Project", Code = "105", Description = "Project description" },
                    new Product { Name = "SharePoint", Code = "106", Description = "SharePoint description" },
                    new Product { Name = "Skype", Code = "107", Description = "Skype description" },
                    new Product { Name = "Teams", Code = "108", Description = "Teams description" },
                    new Product { Name = "Notepad++", Code = "109", Description = "Notepad++ description" },
                    new Product { Name = "Visual Studio Code", Code = "110", Description = "Visual Studio Code description" },
                    new Product { Name = "Visual Studio", Code = "111", Description = "Visual Studio description" },
                    new Product { Name = "Git", Code = "112", Description = "Git description" },
                    new Product { Name = "GitHub", Code = "113", Description = "GitHub description" },
                    new Product { Name = "Sublime Text", Code = "114", Description = "Sublime Text description" },
                    new Product { Name = "MS SqlServer", Code = "115", Description = "MS SqlServer description" },
                    new Product { Name = "Oracle", Code = "116", Description = "MS Access description" },
                    new Product { Name = "MySQL", Code = "117", Description = "MySQL description" },
                    new Product { Name = "PostgreSQL", Code = "118", Description = "PostgreSQL description" },
                    new Product { Name = "MongoDB", Code = "119", Description = "MongoDB description" },
                    new Product { Name = "Docker", Code = "120", Description = "Docker description" },
                    new Product { Name = "Kubernetes", Code = "121", Description = "Kubernetes description" },
                    new Product { Name = "Azure", Code = "122", Description = "Azure description" },
                    new Product { Name = "AWS", Code = "123", Description = "AWS description" }
                ]
            }
        };

        _db.ProductTypes.AddRange(types);
        _db.SaveChanges();
    }
}