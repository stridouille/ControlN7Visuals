using Plots
using Plots.PlotMeasures
using Images

#=
#astroid curve
x(t) = sin(t)^3
y(t) = cos(t)^3
plot(x, y, 0, 2*pi)
=#

#sinus curve
x(t) = sin(t)
plot(x, 0, 2*pi, linecolor=:red)

#use these attributes
plot!(dpi = 1000, framestyle=:none, fmt=:png, background=false, legend=false, grid=false, axis=false, border=false, size=(1000,500), margin=0mm)

#save the plot in desired path
savefig("Assets/Images/output.png")

#these lines crop the image's margins
img_path = "Assets/Images/output.png"
img = load(img_path)
img = img[208:4639, 749:9659]
save("Assets/Images/output.png", img)