import Actors from "./Pages/Actors";
import ActorDetails from "./Pages/ActorDetails";
import Contracts from "./Pages/Contracts";
import Spectacles from "./Pages/Spectacles";

const AppRoutes = [
  {
    path: '/',
    element: <Actors  />
  },
  {
    path: '/spectacles',
    element: <Spectacles/>
  },
  {
    path: '/contracts',
    element: <Contracts/>
  },
  {
    path: '/actor-details',
    element: <ActorDetails/>
  },
];

export default AppRoutes;
