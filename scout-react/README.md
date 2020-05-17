# scout-react
static react app serving up the scout game

built on [react-unity-webgl](https://www.npmjs.com/package/react-unity-webgl)

## steps
build the scout game for [webgl](https://docs.unity3d.com/Manual/webgl-building.html). call the destination `build`.
overwrite the (public/build)[public/Build] directory in this repo with the `build/Build` directory generated above.
run `yarn && yarn start` to make sure everything looks ok
run `yarn build` to generate a production webapp build
from this directory, run `surge build pdavids.space` to push the resultant build to [pdavids.space](http://pdavids.space)
